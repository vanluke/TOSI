using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public static class SecretLoader
    {
        public static byte[] LoadPrivateKeyFile(string filename)//
        {
            using (FileStream fs = File.OpenRead(filename))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    // maybe it's ASCII PEM base64 encoded ? 
                    return PEM("RSA PRIVATE KEY", data);
                }
            }
            return null;
        }
        public static X509Certificate2 LoadCertificateFile(string filename)
        {
            X509Certificate2 x509 = null;
            using (FileStream fs = File.OpenRead(filename))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    // maybe it's ASCII PEM base64 encoded ? 
                    data = PEM("CERTIFICATE", data);
                }
                if (data != null)
                    x509 = new X509Certificate2(data);
            }
            return x509;
        }

        static byte[] PEM(string type, byte[] data)
        {
            string pem = Encoding.ASCII.GetString(data);
            string header = String.Format("-----BEGIN {0}-----", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);
            string base64 = pem.Substring(start, (end - start));
            return Convert.FromBase64String(base64);
        }
    }
}
