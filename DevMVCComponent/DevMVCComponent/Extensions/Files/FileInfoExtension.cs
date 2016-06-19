using System.IO;

namespace DevMvcComponent.Extensions.Files {
    /// <summary>
    ///     File info extensions
    /// </summary>
    public static class FileInfoExtension {
        /// <summary>
        ///     Returns : true if file is being used or being processed by another thread.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Returns : true if file is being used or being processed by another thread.</returns>
        public static bool IsFileLocked(this FileInfo file) {
            FileStream stream = null;
            try {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            } catch (IOException) {
                return true;
            } finally {
                if (stream != null) {
                    stream.Close();
                }
            }

            //file is not locked
            return false;
        }

        /// <summary>
        ///     Returns : true if file is being used or being processed by another thread.
        /// </summary>
        /// <param name="fileName">Absolute file location.</param>
        /// <returns>Returns : true if file is being used or being processed by another thread.</returns>
        public static bool IsFileLocked(this string fileName) {
            var file = new FileInfo(fileName);
            return IsFileLocked(file);
        }
    }
}