using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class Director
    {
        public Director(string path, string privateKeyLocation)
        {
            Certyficate = SecretLoader.LoadCertificateFile(path);
            var privateKeyByte = SecretLoader.LoadPrivateKeyFile(privateKeyLocation);
            var privateKey = Convert.ToBase64String(privateKeyByte);
            Provider = Rsa.DecodeRsaPrivateKey(privateKeyByte);
            Certyficate.PrivateKey = Provider;
        }

        public byte[] SingDate(byte[] dataToSign)
        {
            return Provider.SignData(dataToSign, CryptoConfig.MapNameToOID(Algorithm));
        }

        public bool VerifyData(byte[] dataToCheck, byte[] signedData)
        {
            var key = (RSACryptoServiceProvider)Certyficate.PublicKey.Key;
            return key.VerifyData(dataToCheck, CryptoConfig.MapNameToOID("SHA256"), signedData);
        }

        public X509Certificate2 Certyficate { get; set; }
        public RSACryptoServiceProvider Provider { get; set; }

        private const string Algorithm = "SHA256";
    }
}
