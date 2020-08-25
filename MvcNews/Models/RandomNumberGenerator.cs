using System;
using System.Security.Cryptography;

namespace MvcNews.Models
{
    /// <summary>
    /// This class generates a random key used when creating the salt of a password encryption.
    /// </summary>
    public class RNG
    {

        /// <summary>
        /// Method to generate a salt key based on a configurable length to be returned in text format.
        /// </summary>
        /// <param name="keyLength"></param>
        /// <returns>The salt key in base 64 string format.</returns>
        public string GenerateRandomCryptographicKey(int keylength)
        {
            return Convert.ToBase64String(GenerateRandomCryptographicBytes(keylength));
        }

        /// <summary>
        /// Method to generate a salt key based on a configurable length to be added to a password.
        /// </summary>
        /// <param name="keylength"></param>
        /// <returns>The salt key in bytes.</returns>
        public byte[] GenerateRandomCryptographicBytes(int keylength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keylength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }


    } // End class.

} // End namespace.