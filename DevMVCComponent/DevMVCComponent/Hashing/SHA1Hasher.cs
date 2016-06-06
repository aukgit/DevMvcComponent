#region using block

using System.Linq;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace DevMvcComponent.Hashing {
    /// <summary>
    ///     Generates clean MD5 code.
    /// </summary>
    public class Sha1Hasher : BaseHasher {
        #region Overrides of BaseHasher

        /// <summary>
        ///     Get hash string based on the hasher type
        /// </summary>
        /// <returns></returns>
        public override string GetHash(string input) {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        /// <summary>
        ///     Get file hash string based on the hasher type.
        /// </summary>
        /// <returns></returns>
        public override string GetFileCheckSum(string fileLocation) {
            using (var sha1 = new SHA1CryptoServiceProvider()) {
                using (var fs = GetStream(fileLocation)) {
                    return Hasher.GetFileHash(fs, sha1);
                }
            }
        }

        #endregion
    }
}