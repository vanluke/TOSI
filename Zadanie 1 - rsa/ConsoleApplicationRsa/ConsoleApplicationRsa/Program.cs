using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationRsa
{
    class RSA
    {
        static void Main(string[] args)
        {
            var rsa = new RSA(1024);
            string text1 = "ATTBYAAA";
            BigInteger plaintext = new BigInteger(Encoding.UTF8.GetBytes(text1));
            BigInteger ciphertext = rsa.Encrypt(plaintext);

            var ex = rsa.HashMessage(text1);
            var textres = rsa.Decrypt(ex);
            Console.WriteLine(String.Format("RSA : {0} \n Plain {1} \n Decrypt : {2}", ciphertext, text1, textres));
            Console.ReadKey();
        }
        public RSA(BigInteger newn, BigInteger newe)
        {
            n = newn;
            e = newe;
        }
        private IEnumerable<string> SplitString(string input, int outputStringLength)
        {
            var count = 0;
            while (count < input.Length)
            {
                var length = Math.Min(outputStringLength, input.Length - count);
                var local = string.Format("{0}", input.Substring(count, length));
                while (local.Length < outputStringLength)
                {
                    local += 'Q';
                }
                yield return local;
                count += outputStringLength;
            }
        }
        public IEnumerable<string> DevideIntoBlocks(string message)
        {
            return SplitString(message, blockSize);
        }
        public RSA(int bits)
        {
            bitlen = bits;
            Random r = new Random();
            BigInteger p = BigInteger.Parse(_p);
            BigInteger q = BigInteger.Parse(_q);
            n = p * q;
            BigInteger m = (p - 1) * (q - 1);
            e = new BigInteger(3);
            while (GCD(m, e) > 1)
            {
                e = e + 2;
            }
            d = ModInverse(e, m);
        }

        public List<BigInteger> HashMessage(string msg)
        {

            var castmsg = DevideIntoBlocks(msg.ToUpper()).ToList();
            List<BigInteger> messages = new List<BigInteger>();
            foreach (var element in castmsg)
            {
                BigInteger ret = 0;
                for (int i = element.Length - 1, x = 0; i >= 0; i--, x++)
                {
                    var c = element[x];
                    int value;
                    Alphabet.TryGetValue(c, out value);
                    ret += value * PowBigInteger(26, i);
                }
                messages.Add(PowBigInteger(ret, 3) % n);
            }

            return messages;
        }

        public BigInteger PowBigInteger(BigInteger value, BigInteger powval)
        {
            BigInteger ret = 1;
            for (BigInteger i = 0; i < powval; i++)
            {
                ret *= value;
            }
            return ret;
        }
        
        public string Decrypt(List<BigInteger> value)
        {
            string outstring = String.Empty;
            foreach (var one in value)
            {
                var val = PowBigInteger(one, d) % n;
                for (int i = blockSize - 1, x = 0; i >= 0; i--, x++)
                {
                    var calc = val / PowBigInteger(26, i);
                    var c = GetKeyByValue((int)calc);
                    outstring += c;

                    val = val % PowBigInteger(26, i);
                }
            }

            return outstring;
        }
        public string Encrypt(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var b = new BigInteger(bytes);
            var b2 = BigInteger.ModPow(b, e, n).ToString();
            return b2;
        }
        public BigInteger Encrypt(BigInteger message)
        {
            var b2 = BigInteger.ModPow(message, e, n);
            return b2;
        }

        public BigInteger Decrypt(BigInteger message)
        {
            var b2 = BigInteger.ModPow(message, d, n);
            return b2;
        }

        public string Decrypt(string message)
        {
            var big = BigInteger.Parse(message);
            var modpow = BigInteger.ModPow(big, d, n).ToString();
            return modpow;
        }

        BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

        public BigInteger GCD(BigInteger a, BigInteger b)
        {
            BigInteger Remainder;

            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }
        public char GetKeyByValue(int v)
        {
            return Alphabet.FirstOrDefault(x => x.Value == v).Key;
        }
        private int bitlen = 1024;
        private BigInteger n, d, e;
        private int blockSize = 3;
        private IDictionary<char, int> Alphabet = new Dictionary<char, int>()
        {
            {'A', 0},
            {'B', 1} ,
            {'C', 2},
            {'D', 3},
            {'E', 4},
            {'F', 5},
            {'G', 6},
            {'H', 7},
            {'I', 8},
            {'J', 9},
            {'K', 10},
            {'L', 11},
            {'M', 12},
            {'N', 13},
            {'O', 14},
            {'Q', 15},
            {'P', 16},
            {'R', 17},
            {'S', 18},
            {'T', 19},
            {'U', 20},
            {'V', 21},
            {'W', 22},
            {'X', 23},
            {'Y', 24},
            {'Z', 25},
        };

        private string _p =
            "173";
        // "11345099217501710466106097760465354087674135131513626526611162988822566568468462574449915910910899562271654035571879760186551598300134696258427331866812801";

        private string _q =
            "149"; //290

        // "9516294027300998258822826375385628659856656648862752187378152693544158218726381024778426104008930288348582641853674018903301587564295774259792240051655469";
    }
}
