using System;
using System.IO;
using System.Threading;

namespace DevMvcComponent.Extensions {
    /// <summary>
    /// </summary>
    public static class BinaryFileExtenstion {
        // TODO implement mutex for file saving as object

        /// <summary>
        ///     Save any object into file over the previous one.
        ///     If object is null then don't save anything.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        /// <param name="anyObject">Saving item. Could be array or list or anything.</param>
        public static object ReadfromBinary(this object anyObject, string fileNamelocation) {
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

        /// <summary>
        ///     Save any object into file over the previous one.
        ///     If object is null then don't save anything.
        /// </summary>
        /// <param name="fileNamelocation">Direct file location with it's extension.</param>
        public static T ReadfromBinary2<T>(string fileNamelocation) {
            // write files into binary
            if (File.Exists(fileNamelocation)) {
                try {
                    var fileBytes = File.ReadAllBytes(fileNamelocation);
                    return fileBytes.BinaryToGenericObject<T>();
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            }
            return default(T);
        }
    }
}