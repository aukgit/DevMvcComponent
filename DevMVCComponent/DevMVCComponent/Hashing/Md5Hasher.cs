#region using block

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace DevMvcComponent.Hashing {
    /// <summary>
    ///     Generates clean MD5 code.
    /// </summary>
    public class Md5Hasher : BaseHasher {


        /// <summary>
        /// Get a MD5 checksum byte array from file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public byte[] GetFileCheckSumAsBytes(string fileLocation) {
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(fileLocation)) {
                    return md5.ComputeHash(stream);
                }
            }
        }
        /// <summary>
        /// Get a MD5 checksum byte array from file.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public override string GetFileCheckSum(string fileLocation) {
            using (var fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                using (var md5 = new MD5CryptoServiceProvider()) {
                    return Hasher.GetFileHash(fs, md5);
                }
            }
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

     

        #region Overrides of BaseHasher

        /// <summary>
        /// Get hash string based on the hasher type
        /// </summary>
        /// <returns></returns>
        public override string GetHash(string input) {
            var md5Hash = MD5.Create();
            var coded = GetMd5Hash(md5Hash, input);
            return coded;
        }

        #endregion
    }
}