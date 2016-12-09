using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMaster
{
    class Program
    {
        static void Main(string[] args)
        {
            var encrypted = CBCCrypter.Crypt("ce cours de mathématiques est très intéressant", "7 1 4 5 2 3 8 6");
            Console.WriteLine(encrypted);
            Console.ReadKey();
        }
    }
}
