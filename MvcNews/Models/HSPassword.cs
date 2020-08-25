using System;
using System.Security.Cryptography;

namespace MvcNews.Models
{
    /// <summary>
    /// This entity class returns the result of a hash and salt password.
    /// </summary>
    public class HSPassword
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="digest"></param>
        public HSPassword(string salt, string digest)
        {
            Salt = salt;
            Digest = digest;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public HSPassword() { }


        // Password properties:
        public string Salt { get; }
        public string Digest { get; set; }


        /// <summary>
        /// Method to confirm whether a password is valid or not based on a comparison of hash and salt passwords.
        /// <param name="password"></param>
        /// <param name="saltfromdb"></param>
        /// <param name="hashfromdb"></param>
        /// <returns>Returns true or false depending on a valid or invalid password.</returns>
        public Boolean confirmPassword(string password, string saltFromDB, string hashFromDB)
        {
            byte[] saltBytes = Convert.FromBase64String(saltFromDB);

            PasswordEncryption passwordHasher = new PasswordEncryption();
            HSPassword PasswordHash = passwordHasher.generateHashWithSalt(password, saltBytes, SHA256.Create());

            if (string.Equals(hashFromDB, PasswordHash.Digest))
            {
                return true;
            }
            return false;
        }


    } // End class.

} // End namespace.