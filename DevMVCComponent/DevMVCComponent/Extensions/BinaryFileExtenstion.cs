using System;
using System.IO;
using System.Threading;

namespace DevMvcComponent.Extensions {
    /// <summary>
    /// 
    /// </summary>
    public static class BinaryFileExtenstion {
        /// <summary>
        /// Returns : true if file is being used or being processed by another thread.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Returns : true if file is being used or being processed by another thread.</returns>
        public static bool IsFileLocked(FileInfo file) {
            FileStream stream = null;
            try {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            } catch (IOException) {
                return true;
            } finally {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// Returns : true if file is being used or being processed by another thread.
        /// </summary>
        /// <param name="fileName">Absolute file location.</param>
        /// <returns>Returns : true if file is being used or being processed by another thread.</returns>
        public static bool IsFileLocked(string fileName) {
            var file = new FileInfo(fileName);
            return IsFileLocked(file);
        }

        /// <summary>
        ///     Save any object into file over the previous one.
        ///     If object is null then don't save anything.
        /// </summary>
        /// <param name="fileNamelocation">Should contain extension(ex. text.txt) .Relative file location  from root + additonroot</param>
        /// <param name="anyObject">Could be array or list or anything.</param>
        public static void SaveAsBinary(this object anyObject, string fileNamelocation) {
            if (anyObject == null) {
                return;
            }
            if (File.Exists(fileNamelocation)) {
                File.Delete(fileNamelocation);
            }
            // write files into binary
            try {
                using (var fs = new FileStream(fileNamelocation, FileMode.CreateNew)) {
                    // Create the writer for data.
                    using (var w = new BinaryWriter(fs)) {
                        var binaryObj = anyObject.ToBytesArray();
                        w.Write(binaryObj);
                        w.Close();
                    }
                }
            } catch (Exception ex) {
                Mvc.Error.HandleBy(ex);
            }
        }

        /// <summary>
        ///     Save any object into file over the previous one.
        ///     If object is null then don't save anything.
        ///     Warning: It also checks if the file is locked or not, 
        ///         so if found locked then it will try again with 1 sec interval and continuously for 300 times.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        /// <param name="anyObject">Could be array or list or anything.</param>
        public static void SaveAsBinaryAsync(this object anyObject, string fileNamelocation) {
            int tried = 0;
            const int ableToTry = 300;
            new Thread(() => {
                var isLock = IsFileLocked(fileNamelocation);
                while (isLock) {
                    tried++;
                    Thread.Sleep(1000);
                    if (tried >= ableToTry) {
                        throw new Exception("Can't save the async binary file because the file is locked at " + fileNamelocation);
                    }
                }
                SaveAsBinary(anyObject, fileNamelocation);
            }).Start();
        }

        /// <summary>
        ///     Save any object into file over the previous one.
        ///     If object is null then don't save anything.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        /// <param name="anyObject">Saving item. Could be array or list or anything.</param>
        public static object ReadfromBinary(this object anyObject, string fileNamelocation) {
            if (anyObject == null) {
                return null;
            }
            // write files into binary
            if (File.Exists(fileNamelocation)) {
                try {
                    var fileBytes = File.ReadAllBytes(fileNamelocation);
                    return fileBytes.BinaryToObject();
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return null;
        }
    }
}