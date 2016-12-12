using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;


//Il vous est demandé de coder une classe permettant de chiffrer un message de texte en utilisant
//la méthode Cipher Block Chaining(CBC), et comme fonction de chiffrement des blocs un
//chiffrement par transposition et ce, en utilisant exactement la stratégie décrite dans les notes
//de cours(voir exemple « ce cours de mathématiques est très intéressant »). Nous allons
//cependant introduire une variante afin de simplifier quelque peu le processus.

namespace CryptoMaster
{
    class Program
    {

        //private const string MESSAGE = "ce cours de mathématiques est très intéressant";
        //private const string KEY = "7 1 4 5 2 3 8 6";

        //Bien évidemment, vous aurez besoin d’une classe « Main » ou l’équivalent qui permet
        //d’interagir avec votre classe de chiffrement afin de tester vos algorithmes et vérifier leur bon
        //fonctionnement en affichant à la console ou dans des contrôles utilisateurs les résultats de
        //l’utilisation des méthodes de chiffrement et de déchiffrement.
        static void Main(string[] args)
        {
            //La clé de transposition sera la clé transmise lors du chiffrement et la même clé devra être
            //fournie lors du déchiffrement afin de pouvoir bien ordonner les colonnes et reconstituer le
            //message d’origine ligne par ligne. 

            //N.B.Vous êtes tenu de faire en sorte que l’utilisateur ait à entrer les messages à l’exécution,
            //soit à la console, soit dans une interface utilisateur : on doit voir au minimum l’affichage du
            //message original, crypté et décrypté, afin de pouvoir bien observer que le tout se déroule tel
            //que prévu.

            //Force l'encodage d'entrée à UTF8
            while (true)
            {
                string message = null;
                do
                {
                    message = getMessage();
                } while (message == null);


                string key = null;
                do
                {
                    key = getKey();
                } while (key == null);

                var crypted = CBCCrypter.Crypt(message, key);

                Console.WriteLine("\nLe message crypté =\n" + crypted);

                Console.WriteLine("\nDéchiffrement du message chiffré.");

                string decryptionkey = null;
                do
                {
                    decryptionkey = getKey();
                } while (decryptionkey == null);

                var decrypted = CBCCrypter.Decrypt(crypted, decryptionkey);

                Console.WriteLine("Message déchiffré:\n" + decrypted);

                Console.WriteLine("Appuyez sur une touche pour recommencer...");

                Console.ReadKey();

                Console.Clear();
             
            } 
        }


        public static string getMessage()
        {
            Console.InputEncoding = Encoding.ASCII;
            Console.WriteLine("\nVeuillez entrer le texte à chiffrer (lettres sans accents seulement)");
            string input = Console.ReadLine();

            if (!input.IsASCII())
            {
                Console.WriteLine("Caractères invalides dans la chaîne entrée");
                return null;
            }
            else
            {
                return input;
            } 
        }

        public static string getKey()
        {
            Console.InputEncoding = Encoding.ASCII;
            Console.WriteLine("\nVeuillez entrer la clé de transposition (ex:  7 2 3 1 4 6 5 8)");
            string input = Console.ReadLine();
        
            var tokens = input.Tokens();
            for (var i = 0; i < tokens.Length; i++)
            {
                if (Int32.Parse(tokens[i]) > tokens.Length)
                {
                    Console.WriteLine("Clé de transposition invalide !");
                    return null;
                } 
            }

            return input;
        }
    }
}
