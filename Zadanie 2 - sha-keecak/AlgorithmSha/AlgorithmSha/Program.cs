using System;
using AlgorithmSha.Helpers;

namespace AlgorithmSha
{
    class Program
    {
        static void Main(string[] args)
        {
            ISha3 hash = new Sha3(Permustaions.B1600);
            string result = hash.Create("Mamba", Options.c224);
            string result2 = hash.Create("Mamba", Options.c224);
            Console.WriteLine(result);
            Console.WriteLine(result2);
            Console.ReadKey();
        }
    }
}