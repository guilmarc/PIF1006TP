using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace CryptoMaster
{
    public static class Extensions
    {
        public static bool IsASCII(this string value)
        {
            // ASCII encoding replaces non-ascii with question marks, so we use UTF8 to see if multi-byte sequences are there
            return Encoding.UTF8.GetByteCount(value) == value.Length;
        }

        public static string[] Tokens(this string value)
        {
            return value.Split(' ').Where(x => !x.Equals("")).ToArray();
        }
    }
}