namespace Utilities
{
    /// <summary>
    /// The <c>BCryptHashGenerator</c> class.
    /// It uses BCrypt to encrypt a string with the provided salt.
    /// </summary>
    public class BCryptHashGenerator : IHashing
    {
        /// <inheritdoc/>
        public string GenerateHashedString(string stringToBeHashed, string salt)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(stringToBeHashed, salt);
            return hashedPassword;
        }

        /// <inheritdoc/>
        public string GenerateSalt()
        {            
            return BCrypt.Net.BCrypt.GenerateSalt(8);
        }
    }
}
