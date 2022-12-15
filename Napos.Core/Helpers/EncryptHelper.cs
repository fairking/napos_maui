using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Napos.Core.Helpers
{
    public static class EncryptHelper
    {
        /// <summary>
        /// Creates a password hash
        /// </summary>
        /// <param name="pwd">Password or api key</param>
        /// <param name="salt">Use EncryptHelper.CreateSalt in order to create a salt. Store salt among with password hash.</param>
        /// <returns>Returns hash</returns>
        public static string CreateHash(string pwd, string salt = null)
        {
            if (pwd.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(pwd));

            // Convert string to bytes
            var pwdBytes = Encoding.UTF8.GetBytes(pwd);
            var saltBytes = !salt.IsNullOrEmpty() ? Encoding.UTF8.GetBytes(salt) : null;

            // Combine pwd and salt together
            byte[] combinedPwdSalt = saltBytes != null ? MixByteArrays(pwdBytes, saltBytes) : pwdBytes;

            // Compute hash
            using (var algorithm = SHA512.Create())
            {
                var hashBytes = algorithm.ComputeHash(combinedPwdSalt);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static byte[] MixByteArrays(byte[] one, byte[] two)
        {
            var oneQueue = new Queue<byte>(one);
            var twoQueue = new Queue<byte>(two);

            byte[] mix = new byte[one.Length + two.Length];

            for (int i = 0; i < mix.Length; i++)
            {
                if (i % 2 == 0)
                    mix[i] = oneQueue.Count > 0 ? oneQueue.Dequeue() : twoQueue.Dequeue();
                else
                    mix[i] = twoQueue.Count > 0 ? twoQueue.Dequeue() : oneQueue.Dequeue();
            }

            return mix;
        }
    }
}
