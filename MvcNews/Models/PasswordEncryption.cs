using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MvcNews.Models
{
    /// <summary>
    /// This class enables the hashing of a password with salt.
    /// </summary>
    public class PasswordEncryption
    {

        /// <summary>
        /// Method to hash a password with salt.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hashAlgo"></param>
        /// <returns>The hashed password in a base 64 string format.</returns>
        public HSPassword generateHashWithSalt(string password, byte[] salt, HashAlgorithm hashAlgo)
        {
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(salt);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HSPassword(Convert.ToBase64String(salt), Convert.ToBase64String(digestBytes));
        }

        /// <summary>
        /// Method to randomly generate a salt with a configurable length to be added to a password.
        /// </summary>
        /// <param name="saltlength"></param>
        /// <returns>The generated salt in bytes.</returns>
        public byte[] generateSalt(int saltlength)
        {
            RNG rng = new RNG();
            byte[] saltBytes = rng.GenerateRandomCryptographicBytes(saltlength);

            return saltBytes;
        }
        

    } // End class.

} // End namespace.