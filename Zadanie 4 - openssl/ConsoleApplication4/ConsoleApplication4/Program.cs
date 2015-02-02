using System;
using System.Text;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            var director = new Director(Path, PathpriveteKey);

            byte[] data = Encoding.UTF8.GetBytes("asdasdsad");
            byte[] data2 = Encoding.UTF8.GetBytes("asdasdsad");
            var signedData = director.SingDate(data);
            var result = director.VerifyData(data2, signedData);

            Console.WriteLine(String.Format("Verify result : {0}",result));
            Console.ReadKey();
        }

        /*
         Możesz wygenerować własne certyfikaty z openssl lub skorzystać z moich w filderze certyficate
         */

        private const string Path =
            @"...    sciezka do  => self-signed-cert.pem";
        private const string PathpriveteKey =
            @"...    sciezka do  => rsa-private-key.pem";
    }
}