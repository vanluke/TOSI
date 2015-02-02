using AlgorithmSha.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmSha
{
    public class Sha3 : ISha3
    {
        public Sha3(Permustaions b)
        {
            w = ((int)b) / 25;
            l = (Convert.ToInt32(Math.Log(w, 2))); // from double to integer.
            n = 12 + 2 * l;
        }

        public string Create(string text, Options options)
        {
            return Create(text, options.BlockSize, options.Capacities, options.Diversifier);
        }
        internal ulong Rot(ulong value, int offset)
        {
            return (value << offset) | (value >> (64 - offset));
        }

        private ulong[,] RoundB(ulong[,] A, ulong RC)
        {
            var c = new ulong[5];
            var d = new ulong[5];
            var b = new ulong[5, 5];
            // ---->  θ Etap : theta
            Theta(A, c, d);
            //----> ρ  π Etap : Rho and Pi
            RhoAndPi(A, b);        
            //----> χ Etap: Chi
            Chi(A, b);      
            //---->  ι Etap : Iota
            Iota(A, RC);

            return A;
        }

        private static void Iota(ulong[,] a, ulong rc)
        {
            a[0, 0] = a[0, 0] ^ rc;
        }

        private static void Chi(ulong[,] a, ulong[,] b)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    a[i, j] = b[i, j] ^ ((~b[(i + 1)%5, j]) & b[(i + 2)%5, j]);
                }
            }
        }

        private void RhoAndPi(ulong[,] a, ulong[,] b)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    b[j, (2*i + 3*j)%5] = Rot(a[i, j], Shared.R[i, j]);
                }
            }
        }

        private void Theta(ulong[,] a, ulong[] c, ulong[] d)
        {
            for (int i = 0; i < 5; i++)
            {
                c[i] = a[i, 0] ^ a[i, 1] ^ a[i, 2] ^ a[i, 3] ^ a[i, 4];
            }

            for (int i = 0; i < 5; i++)
            {
                d[i] = c[(i + 4)%5] ^ Rot(c[(i + 1)%5], 1);
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    a[i, j] = a[i, j] ^ d[i];
                }
            }
        }

        private ulong[,] Keccackf(ulong[,] a)
        {
            for (int i = 0; i < n; i++)
            {
                a = RoundB(a, Shared.RC[i]);
            }

            return a;
        }
        /// <summary>
        /// Dodaje bites- bitow i Konwersja do 64 - bitowych słów.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="bites"></param>
        /// <returns></returns>
        private ulong[][] PerformeData(string one, int bites)
        {
            int size = 0;
            StringBuilder builder = new StringBuilder(one);
            // Uzupełenie.
            builder.Append("01");
            while (((builder.Length / 2) * 8 % bites) != ((bites - 8)))
            {
                builder.Append("00");
            };
            builder.Append("80");
            //Ilosc blokow
            size = (((builder.Length / 2) * 8) / bites);

            ulong[][] retval = new ulong[size][];
            retval[0] = new ulong[1600 / w];
            string temp = String.Empty;
            int count = 0;
            int j = 0;
            int i = 0;
            var str = builder.ToString();
            //Konwersja ciągu znaków z tablicy 64-bitowych słów
            foreach (char ch in str)
            {
                if (j > (bites / w - 1))
                {
                    j = 0;
                    i++;
                    retval[i] = new ulong[1600 / w];
                }
                count++;
                if ((count * 4 % w) == 0)
                {
                    retval[i][j] = Convert.ToUInt64(str.Substring((count - w / 4), w / 4), 16);
                    temp = ReverseHexString(retval[i][j]);
                    retval[i][j] = Convert.ToUInt64(temp, 16);
                    j++;
                }

            }
            return retval;
        }

        /// <summary>
        /// block size, capacities  (c=448, 512, 768 and 1024), diversifier
        /// </summary>
        /// <param name="M"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private string Create(string text, int blockSize, int capacitie, int diversifier)
        {
            text = PrepareText(text);
            // inicjacja tablicy s
            var s = InitS();

            // tablica elementow pi. Kazdy element 64 - bity
            ulong[][] P = PerformeData(text, blockSize);

            foreach (ulong[] Pi in P)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if ((i + j * 5) < (blockSize / w))
                        {
                            s[i, j] = s[i, j] ^ Pi[i + j * 5];
                        }
                    }
                }
                Keccackf(s);
            }
            string Z = String.Empty;
            // wykonuje petle do while do momentu az osiągnę założoną długość.
            while (Z.Length < diversifier * 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if ((5 * i + j) < (blockSize / w))
                        {
                            Z = Z + ReverseHexString(s[j, i]);
                        }
                    }

                }
                Keccackf(s);
            }
            return Z.Substring(0, diversifier * 2);
        }

        private ulong[,] InitS()
        {
            ulong[,] S = new ulong[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    S[i, j] = 0;
                }
            }
            return S;
        }

        private string PrepareText(string text)
        {
            return BitConverter.ToString(Encoding.Default.GetBytes(text).ToArray()).Replace("-", "");
        }

        private string ReverseHexString(ulong value)
        {
            return BitConverter.ToString(BitConverter.GetBytes(value).ToArray()).Replace("-", "");
        }

        private string HexString(ulong value)
        {
            return BitConverter.ToString(BitConverter.GetBytes(value).Reverse().ToArray()).Replace("-", "");
        }

        private int w;
        private int l;
        private int n;
    }
}
