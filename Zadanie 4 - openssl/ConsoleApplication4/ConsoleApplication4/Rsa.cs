using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace ConsoleApplication4
{
    public class Rsa
    {
        public static RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privateKeyBytes)
        {
            MemoryStream ms = new MemoryStream(privateKeyBytes);
            BinaryReader rd = new BinaryReader(ms);

            try
            {
                byte byteValue;
                ushort shortValue;

                shortValue = rd.ReadUInt16();

                switch (shortValue)
                {
                    case 0x8130:
                        // If true, data is little endian since the proper logical seq is 0x30 0x81
                        rd.ReadByte(); //advance 1 byte
                        break;
                    case 0x8230:
                        rd.ReadInt16();  //advance 2 bytes
                        break;
                    default:
                        Debug.Assert(false);     // Improper ASN.1 format
                        return null;
                }

                shortValue = rd.ReadUInt16();
                if (shortValue != 0x0102) // (version number)
                {
                    Debug.Assert(false);     // Improper ASN.1 format, unexpected version number
                    return null;
                }

                byteValue = rd.ReadByte();
                if (byteValue != 0x00)
                {
                    Debug.Assert(false);     // Improper ASN.1 format
                    return null;
                }

                CspParameters parms = new CspParameters();
                parms.Flags = CspProviderFlags.NoFlags;
                parms.KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant();
                parms.ProviderType = ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1))) ? 0x18 : 1;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(parms);
                RSAParameters rsAparams = new RSAParameters();

                rsAparams.Modulus = rd.ReadBytes(Extractor.DecodeIntegerSize(rd));

                RsaTraits traits = new RsaTraits(rsAparams.Modulus.Length * 8);

                rsAparams.Modulus = Extractor.AlignBytes(rsAparams.Modulus, traits.SizeMod);
                rsAparams.Exponent = Extractor.AlignBytes(rd.ReadBytes(Extractor.DecodeIntegerSize(rd)), traits.SizeExp);
                rsAparams.D = Extractor.AlignBytes(rd.ReadBytes(Extractor.DecodeIntegerSize(rd)), traits.SizeD);
                rsAparams.P = Extractor.AlignBytes(rd.ReadBytes(Extractor.DecodeIntegerSize(rd)), traits.SizeP);
                rsAparams.Q = Extractor.AlignBytes(rd.ReadBytes(Extractor.DecodeIntegerSize(rd)), traits.SizeQ);
                rsAparams.DP = Extractor.AlignBytes(rd.ReadBytes(Extractor.DecodeIntegerSize(rd)), traits.SizeDp);
                rsAparams.DQ = Extractor.AlignBytes(rd.ReadBytes(Extractor.DecodeIntegerSize(rd)), traits.SizeDq);
                rsAparams.InverseQ = Extractor.AlignBytes(rd.ReadBytes(Extractor.DecodeIntegerSize(rd)), traits.SizeInvQ);

                rsa.ImportParameters(rsAparams);
                return rsa;
            }
            catch (Exception)
            {
                Debug.Assert(false);
                return null;
            }
            finally
            {
                rd.Close();
            }
        }
    }
}
