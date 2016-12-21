using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace CryptoMaster
{
    public class CBCCrypter
    {

        //Le vecteur d’initialisation(VI), si vous le souhaitez, des raisons de simplification, peut être
        //prédéterminé et le même à l’intérieur des méthodes de chiffrement et déchiffrement et non
        //pas déterminé au hasard.

        private static byte INIT_VECTOR = 0x0A;
        private static byte PADDING = 0x00;

        //Dans un premier temps, le message au complet devra subir un chiffrement par
        //transposition en utilisant une clé de transposition au choix de l’utilisateur, composée
        //d’une chaîne de caractères composée d’une série de nombres séparés par un caractère
        //d’espacement(p.ex. « 1 4 6 5 3 2 »). La quantité de nombres dans la série donnera par
        //le fait même le nombre de colonnes de transposition.
        //Considérez que l’utilisateur entrera une série adéquate.
        //- Dans un deuxième temps, vous devez utiliser la méthode de chiffrement par bloc CBC
        //sur le message précédemment transposé en considérant que 1 caractère (1 octet) = 1
        //bloc.La clé(fonction) de chiffrement ayant déjà été appliquée préalablement sur le
        //message en entier, vous n’avez qu’à effectuer l’opération XOR entre la valeur du bloc
        //clair avec la valeur du bloc chiffré précédent(ou le vecteur d’initialisation pour le
        //premier bloc à chiffrer).
        //Cela implique que vous devrez transformer la chaîne de caractères comportant le
        //message en un tableau d’octets(byte[]) qui constituera l’ensemble des blocs clairs.Le
        //tableau d’octets chiffrés que vous aurez produit suite au chiffrement par CBC devra être
        //reformé en une chaîne de caractère que vous retournerez à l’appelant et qui formera
        //ainsi le message chiffré. 

        //T[n] = nième bloc de texte clair
        //C[n] = nième bloc de texte chiffé
        //E(m) = fonction de chiffrement du bloc m
        //D(m) = fonction de déchiffrement du bloc m
        //VI = Vecteur d'initialisation

        /// <summary>
        /// Méthode cryptant un message à l'aide de la méthode de Cipher Block Chaining
        /// et par transposition interne
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Crypt(string message, string key)
        {
            var transposed = getTransposed(message, key);

            var T = CBCEncoding.GetBytes(transposed);
            var C = new byte[T.Length];

            //Voici pourquoi des operations sur deux bytes retournent un int : https://blogs.msdn.microsoft.com/oldnewthing/20040310-00/?p=40323
            C[0] = E((byte) (T[0] ^ INIT_VECTOR));

            for (var n = 1; n < T.Length; n++)
            {
                C[n] = E((byte)(T[n] ^ C[n - 1]));
            }

            return CBCEncoding.GetString(C);
        }

        private static Encoding CBCEncoding
        {
            get { return Encoding.ASCII; }
        }

        private static byte E(byte value)
        {
            return (byte)(value - 1);
        }

        private static byte D(byte value)
        {
            return (byte) (value + 1);
        }

        private static string getPadding()
        {
            return CBCEncoding.GetString(new byte[] {PADDING});
        }

        public static string getTransposed(string message, string key)
        {
            var transposed = "";
            string[] tokens = key.Tokens();
            var columns = tokens.Length;
            var rows = (int)Math.Ceiling(message.Length/(double) columns);

            for (var i = 1; i <= columns; i++)
            {
                var column = Array.IndexOf(tokens, i.ToString());

                for (var j = 0; j < rows; j++)
                {
                    var index = j * columns + column;
                    transposed += index <= (message.Length - 1) ? message[index].ToString() : getPadding();
                }
            }
            return transposed;
        }

        public static string getTransposedInv(string message, string key)
        {
            var transposedInv = "";
            var tokens = key.Tokens();
            var rows = tokens.Length;
            var columns = (int)Math.Ceiling(message.Length / (double)rows);

            for (var column = 0; column < columns; column++)
            {
                for (var row = 0; row < rows; row++)
                {

                    var position = int.Parse(tokens[row]) - 1;
                    var index = (position*columns) + column;
                    if (index < message.Length)
                    {
                        transposedInv += message[index].ToString() == getPadding() ? "" : message[index].ToString();
                    }
                    
                }
            }

            return transposedInv;

        }


        //Vous devez d’abord décomposé le message chiffré en un tableau d’octets, sur lequel
        //vous appliquez l’algorithme de déchiffrement avec la méthode CBC sur chacun des blocs
        //d’octet.Cela aura alors pour effet de reconstituer le message transposé.
        //- Puis, vous devez appliquer la transposition inverse en respectant la clé de transposition
        //fournie par l’utilisateur et retourner le message résultant. Avec la même clé de
        //transposition utilisée pour le chiffrement (et un peu de « chance » !), le message
        //retourné devrait être celui d’origine.

        /// <summary>
        /// Méthode decryptant un message préalablement crypté à l'aide de la méthode ce
        /// Cipher Block Chaining et par transposition interne
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string message, string key)
        {
            var C = CBCEncoding.GetBytes(message);
            var T = new byte[C.Length];

            T[0] = D( (byte)(C[0] ^ INIT_VECTOR) );

            for (var n = 1; n < C.Length; n++)
            {
                T[n] = (byte)(D( C[n] ) ^ C[n - 1]);
            }

            return getTransposedInv(CBCEncoding.GetString(T), key);
        }

    }
}

//Enfin, vous pourriez avoir besoin des éléments qui suivent pour mener à bien votre travail :

//- L’opérateur XOR en C# est « ^ » et doit être utilisé sur des variables de types int ou des
//bits(valeurs booléennes) (vous aurez donc probablement des opérations de casting à faire);

//- Pour passer d’un String vers un byte[], une des façons de faire consiste à instancier
//un objet de classe ASCIIEncoding ou Encoding et appeler la méthode GetBytes().

//- Pour transformer un String en un tableau de jetons(sous-chaînes), vous pouvez utiliser
//la méthode Split() et spécifier un séparateur(p.ex.un caractère d’espacement).