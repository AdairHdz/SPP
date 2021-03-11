using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HashingTests
{
    [TestClass]
    public class BCryptHashingTest
    {
        [DataRow("abcdefghijk")]
        [DataRow("abcdefghijklmnopqrstuvwxyz")]
        [DataRow("SADFKfsjvcxmxcZ:_dses$%489439eiwdsqassAASDc.krdjgfdde594865ir3w")]
        [DataRow("contraseña insegura")]
        [DataRow("adair benjamin hernandez ortiz")]
        [DataRow("Contraseña123")]
        [TestMethod]
        public void CompareHashedPassword_Matches(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            bool expected = true;
            bool actual = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            Assert.AreEqual(expected, actual);
        }

        [DataRow("abcdefghijk")]
        [DataRow("abcdefghijklmnopqrstuvwxyz")]
        [DataRow("SADFKfsjvcxmxcZ:_dses$%489439eiwdsqassAASDc.krdjgfdde594865ir3w")]
        [DataRow("contraseña insegura")]
        [DataRow("adair benjamin hernandez ortiz")]
        [DataRow("Contraseña123")]
        [TestMethod]
        public void CompareHashedPassword_MatchesWithSalt(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(8);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            bool expected = true;
            bool actual = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            Assert.AreEqual(expected, actual);
        }

        [DataRow("abcdefghijk")]
        [DataRow("abcdefghijklmnopqrstuvwxyz")]
        [DataRow("SADFKfsjvcxmxcZ:_dses$%489439eiwdsqassAASDc.krdjgfdde594865ir3w")]
        [DataRow("contraseña insegura")]
        [DataRow("adair benjamin hernandez ortiz")]
        [DataRow("Contraseña123")]
        [TestMethod]
        public void CompareHashedPassword_DoesNotMatch(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            bool expected = false;
            bool actual = BCrypt.Net.BCrypt.Verify(password + "abc", hashedPassword);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CompareEncryptedPasswords_Success()
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(8);

            string firstPassword = "adairbenjamin";
            string firstHashedPassword = BCrypt.Net.BCrypt.HashPassword(firstPassword, salt);
            string secondPassword = "adairbenjamin";
            string secondHashedPassword = BCrypt.Net.BCrypt.HashPassword(secondPassword, salt);

            Assert.AreEqual(firstHashedPassword, secondHashedPassword);
        }
    }
}
