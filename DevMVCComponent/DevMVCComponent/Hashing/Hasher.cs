using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DevMvcComponent.Hashing {
    /// <summary>
    ///     Hash any thing or set of arguments
    /// </summary>
    public static class Hasher {
        /// <summary>
        ///     Checks nulls and returns only codes for existing ones.
        /// </summary>
        /// <param name="o">Your hashing parameters</param>
        /// <returns></returns>
        public static string Get(params object[] o) {
            var sb = new StringBuilder(o.Length);
            for (var i = 0; i < o.Length; i++) {
                if (o[i] != null) {
                    sb.Append(o[i].GetHashCode() + "_");
                }
            }
            return sb.ToString().GetHashCode().ToString();
        }

        /// <summary>
        ///     Get any file hash with given parameters.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="hasher"></param>
        /// <returns></returns>
        public static string GetFileHash(Stream s, HashAlgorithm hasher) {
            var hash = hasher.ComputeHash(s);
            var hashStr = Convert.ToBase64String(hash);
            return hashStr.TrimEnd('=');
        }

        /// <summary>
        ///     Checks nulls and returns only codes for existing ones.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5(string input) {
            var hasher = new Md5Hasher();
            return hasher.GetHash(input);
        }

        /// <summary>
        ///     Checks nulls and returns only codes for existing ones.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSha1(string input) {
            var hasher = new Sha1Hasher();
            return hasher.GetHash(input);
        }

        /// <summary>
        ///     Checks previous has with current hash token via Cookie.
        /// </summary>
        /// <param name="cookieName">A unique cookie name from your application to check if this current Hash is same as previous.</param>
        /// <param name="o">Your hashing parameters</param>
        /// <returns></returns>
        public static bool IsSameUsingCookie(string cookieName, params object[] o) {
            var currentHash = Get(o);
            var previous = Mvc.Cookies[cookieName];
            if (previous != null && previous.Equals(currentHash)) {
                // is same.
                return true;
            }
            Mvc.Cookies[cookieName] = currentHash;
            return false;
        }

        /// <summary>
        ///     Checks previous hash with current hash token via cache.
        ///     Warning: Cache is application specific, to have client specific cache use parameter to pass client id or user id.
        /// </summary>
        /// <param name="cacheName">A unique cache name from your application to check if this current Hash is same as previous.</param>
        /// <param name="o">Your hashing parameters</param>
        /// <returns></returns>
        public static bool IsSameUsingCache(string cacheName, params object[] o) {
            var currentHash = Get(o);
            var previous = Mvc.Caches[cacheName];
            if (previous != null && previous.Equals(currentHash)) {
                // is same.
                return true;
            }
            Mvc.Cookies[cacheName] = currentHash;
            return false;
        }
    }
}