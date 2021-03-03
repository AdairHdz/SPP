using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HashingTests
{
    [TestClass]
    public class BCryptHasherTest
    {
        [DataRow("abcdefghijk", 8)]
        [DataRow("abcdefghijklmnopqrstuvwxyz", 8)]
        [DataRow("SADFKfsjvcxmxcZ:_dses$%489439eiwdsqassAASDc.krdjgfdde594865ir3w", 8)]
        [DataRow("contraseña insegura", 6)]
        [DataRow("adair benjamin hernandez ortiz", 6)]
        [DataRow("Contraseña123", 5)]
        [TestMethod]
        public void CompareHashedPassword_Matches(string password, int cost)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(cost);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            bool expected = true;
            bool actual = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            Assert.AreEqual(expected, actual);

        }

        [DataRow("holamundo", "holamundo", 8)]
        [DataRow("adairbenjamin", "adairbenjamin", 6)]
        [DataRow("contraseña123", "contraseña123", 7)]
        [DataRow("kfsjgnfo5847t856o", "kfsjgnfo5847t856o", 5)]
        [DataRow("klfsvkbndgjknkfccsañdew+a´+d43959340", "klfsvkbndgjknkfccsañdew+a´+d43959340", 4)]
        [TestMethod]
        public void CompareEncryptedPasswords_Success(string firstPassword, string secondPassword, int cost)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(cost);
            string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(firstPassword, salt);            
            string secondEncryptedPassword = BCrypt.Net.BCrypt.HashPassword(secondPassword, salt);

            Assert.AreEqual(encryptedPassword, secondEncryptedPassword);
        }
    }
}
