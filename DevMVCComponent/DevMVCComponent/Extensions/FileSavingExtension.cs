using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DevMvcComponent.Extensions {
    /// <summary>
    ///     File saving extension
    /// </summary>
    public static class FileSavingExtension {
        /// <summary>
        /// </summary>
        private static readonly Dictionary<int, Mutex> MutexCollection = new Dictionary<int, Mutex>(100);

        private static Mutex GetMutex(int key) {
            if (!MutexCollection.ContainsKey(key)) {
                MutexCollection[key] = new Mutex(false, key.ToString());
            }
            var mutex = MutexCollection[key];
            if (mutex == null) {
                MutexCollection[key] = new Mutex(false, key.ToString());
                mutex = MutexCollection[key];
            }
            return mutex;
        }
        private static void MutexDisposed(int key) {
            if (MutexCollection.ContainsKey(key)) {
                var mutex = MutexCollection[key];
                mutex.ReleaseMutex();
                mutex.Dispose();
                MutexCollection.Remove(key);
            }
        }

        /// <summary>
        ///     Save any object into file over the previous one.
        ///     If object is null then don't save anything.
        ///     Warning: It also checks if the file is locked or not,
        ///     so if found locked then it will try again with 1 sec interval and continuously for 300 times.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        /// <param name="anyObject">Could be array or list or anything.</param>
        /// <param name="removeIfExist">remove the file if already exist</param>
        public static void SaveAsBinary(this object anyObject, string fileNamelocation, bool removeIfExist = true) {
            if (anyObject == null) {
                return;
            }
            var key = fileNamelocation.GetHashCode();
            var mutex = GetMutex(key);
            mutex.WaitOne();
            if (removeIfExist) {
                if (File.Exists(fileNamelocation)) {
                    File.Delete(fileNamelocation);
                }
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
            } finally {
                MutexDisposed(key);
            }
        }

        /// <summary>
        ///     Save any object into file over the previous one.
        ///     If object is null then don't save anything.
        ///     Warning: It also checks if the file is locked or not,
        ///     so if found locked then it will try again with 1 sec interval and continuously for 300 times.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        /// <param name="str"></param>
        /// <param name="removeIfExist">remove the file if already exist</param>
        /// <param name="append"></param>
        public static void SaveAsString(this string str, string fileNamelocation, bool removeIfExist = true, bool append = false) {
            if (str == null) {
                return;
            }
            var key = fileNamelocation.GetHashCode();
            var mutex = GetMutex(key);
            mutex.WaitOne();
            if (removeIfExist) {
                if (File.Exists(fileNamelocation)) {
                    File.Delete(fileNamelocation);
                }
            }
            try {
                if (append == false) {
                    File.WriteAllText(fileNamelocation, str);
                } else {
                    File.AppendAllText(fileNamelocation, str);
                }
            } catch (Exception ex) {
                Mvc.Error.HandleBy(ex);
            } finally {
                MutexDisposed(key);
            }
        }

        /// <summary>
        ///     Save any object into file over the previous one.
        ///     If object is null then don't save anything.
        ///     Warning: It also checks if the file is locked or not,
        ///     so if found locked then it will try again with 1 sec interval and continuously for 300 times.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        /// <param name="str"></param>
        /// <param name="removeIfExist">remove the file if already exist</param>
        /// <param name="append"></param>
        public static void SaveAsString(this IEnumerable<string> str, string fileNamelocation, bool removeIfExist = true, bool append = false) {
            if (str == null) {
                return;
            }
            var key = fileNamelocation.GetHashCode();
            var mutex = GetMutex(key);
            mutex.WaitOne();
            if (removeIfExist) {
                if (File.Exists(fileNamelocation)) {
                    File.Delete(fileNamelocation);
                }
            }
            try {
                if (append == false) {
                    File.WriteAllLines(fileNamelocation, str);
                } else {
                    File.AppendAllLines(fileNamelocation, str);
                }
            } catch (Exception ex) {
                Mvc.Error.HandleBy(ex);
            } finally {
                MutexDisposed(key);
            }
        }
        /// <summary>
        ///     Read binary to explicit object.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        /// <param name="anyObject">Could be array or list or anything.</param>
        public static T ReadBinaryAs<T>(this T anyObject, string fileNamelocation) {
            return ReadBinaryAs<T>(fileNamelocation);
        }


        /// <summary>
        ///     Read binary to explicit object.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        public static T ReadBinaryAs<T>(string fileNamelocation) {
            var key = fileNamelocation.GetHashCode();
            var mutex = GetMutex(key);
            mutex.WaitOne();
            // write files into binary
            if (File.Exists(fileNamelocation)) {
                try {
                    var fileBytes = File.ReadAllBytes(fileNamelocation);
                    MutexDisposed(key);
                    return (T) fileBytes.BinaryToObject();
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                } finally {
                    MutexDisposed(key);
                }
            }
            return default(T);
        }
    }
}