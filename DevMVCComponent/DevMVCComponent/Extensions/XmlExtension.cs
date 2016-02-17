using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace DevMvcComponent.Extensions {
    public static class XmlExtension {
        private static readonly Object SyncObj = new Object();
        private static Mutex Mutex = null;

        
        public static bool WriteXmlToFile<T>(this T obj, string absoluteFilePath , string mutextName = "devmvc-component-xml", bool globalLock = true, bool internalLock = true) {
            if (globalLock) {
                Mutex.WaitOne();
            }
            var fileLocation = absoluteFilePath;
            try {
                if (internalLock) {
                    lock (SyncObj) {
                        WriteSerializedObject(fileLocation, obj); // Write with internal lock
                    }
                } else {
                    WriteSerializedObject(fileLocation, obj);// Write without internal lock
                }
            } catch (Exception ex) {
                return false;
            } finally {
                if (globalLock) {
                    Mutex.ReleaseMutex();
                }
            }
            return true;
        }
        public static string ToXmlString<T>(this T toSerialize) {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using (StringWriter textWriter = new StringWriter()) {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}