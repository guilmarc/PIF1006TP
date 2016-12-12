using System;
using CryptoMaster;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptoMasterUnitTest
{
    [TestClass]
    public class UnitTest
    {
        private const string MESSAGE = "ce cours de mathematiques est tres interessant";
        private const string KEY = "7 1 4 5 2 3 8 6";

        [TestMethod]
        public void TestTransposed()
        {
            var transposed = CBCCrypter.getTransposed(MESSAGE, KEY);
            var transposedInv = CBCCrypter.getTransposedInv(transposed, KEY);
            Assert.IsTrue(transposedInv.Equals(MESSAGE));
        }

        [TestMethod]
        public void TestEncryption()
        {
            var encrypted = CBCCrypter.Crypt(MESSAGE, KEY);
            var decrypted = CBCCrypter.Decrypt(encrypted, KEY);
            Assert.IsTrue(decrypted.Equals(MESSAGE));
        }
    }
}
