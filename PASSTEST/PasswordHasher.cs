using System;
using System.Security.Cryptography;

namespace PASSTEST
{    
    ///<summary>
    /// PasswordHasher modified by Rnmkr
    /// from original "SecurePasswordHasher"
    /// by Christian Gollhardt post at
    /// https://stackoverflow.com/a/32191537
    /// </summary>

    public sealed class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public static string Hash(string password)
        {
            return Hash(password, Iterations);
        }
        
        public static string Hash(string password, int Iterations)
        {
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            //return hashed password + salt
            return string.Format(base64Hash);
        }

        public static bool Verify(string password, string hashedPassword)
        {
            var base64Hash = hashedPassword;

            //get hashbytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
