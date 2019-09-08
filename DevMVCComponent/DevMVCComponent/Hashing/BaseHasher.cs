using System;
using System.IO;

namespace DevMvcComponent.Hashing
{
    /// <summary>
    ///     Basic Hasher
    /// </summary>
    public abstract class BaseHasher
    {
        /// <summary>
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        protected Stream GetStream(string fileLocation) =>
            new FileStream(
                fileLocation,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite);

        /// <summary>
        ///     Get hash string based on the hasher type
        /// </summary>
        /// <returns></returns>
        public abstract string GetHash(string input);

        /// <summary>
        ///     Get file hash string based on the hasher type.
        /// </summary>
        /// <returns></returns>
        public abstract string GetFileCheckSum(string fileLocation);

        /// <summary>
        ///     Verify hash based on the hasher type
        /// </summary>
        /// <returns></returns>
        public bool VerifyHash(string previousHash, string currentInput)
        {
            var hashOfInput = GetHash(currentInput);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, previousHash))
            {
                return true;
            }

            return false;
        }
    }
}