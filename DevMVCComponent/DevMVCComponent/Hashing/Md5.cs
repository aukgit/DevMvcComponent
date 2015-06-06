#region using block

using System;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace DevMVCComponent.Hashing {
    /// <summary>
    ///     Generates clean MD5 code.
    /// </summary>
    public class Md5 {
        //Generate MD5 Code
        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GenerateCleanMd5(string input) {
            string coded;
            var md5Hash = MD5.Create();
            coded = GetMd5Hash(md5Hash, input);
            return coded;
        }

        /// <summary>
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetMd5Hash(MD5 md5Hash, string input) {
            // Convert the input string to a byte array and compute the hash.
            var data = md5Hash.ComputeHash(Encoding.UTF32.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (var i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        /// <summary>
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash) {
            // Hash the input.
            var hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash)) {
                return true;
            }
            return false;
        }
    }
}