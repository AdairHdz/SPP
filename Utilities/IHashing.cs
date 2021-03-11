namespace Utilities
{
    /// <summary>
    /// The <c>IEncryption</c> interface.
    /// Defines the signatures that its implementers must address-
    /// </summary>
    public interface IHashing
    {
        /// <summary>
        /// Generates an encrypted string from the original string and a salt.
        /// </summary>
        /// <param name="stringToBeEncrypted">The original string to be encrypted.</param>
        /// <param name="salt">The salt that is going to be used to encrypt the string</param>
        /// <returns>
        /// The encrypted string.
        /// </returns>
        string GenerateHashedString(string stringToBeEncrypted, string salt);

        /// <summary>
        /// Generates a salt to encrypt the string.
        /// </summary>
        /// <returns>
        /// The salt to encrypt the password.
        /// </returns>
        string GenerateSalt();
    }
}
