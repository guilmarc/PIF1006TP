using System;
using System.Linq;
using System.Threading;

namespace CryptoMaster
{
    public class CBCCrypter
    {


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
            
             
            

            return transposed;
        }

        private static String getTransposed(string message, string key)
        {
            var transposed = "";
            var tokens = key.Split(' ');
            var columns = tokens.Count();
            var rows = Math.Floor(message.Count()/(double) columns);

            for (var i = 1; i <= columns; i++)
            {
                int column = Array.IndexOf(tokens, i.ToString());

                for (var j = 0; j <= rows; j++)
                {
                    int index = j * columns + column;
                    transposed += index <= (message.Count() - 1) ? message[index].ToString() : " ";
                }
            }
            return transposed;
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
            return "";
        }

    }
}