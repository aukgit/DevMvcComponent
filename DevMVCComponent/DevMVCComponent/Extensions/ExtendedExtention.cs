#region using block

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using DevMvcComponent.EntityType;

#endregion

namespace DevMvcComponent.Extensions {
    /// <summary>
    ///     Get the list of classes to exporting format
    /// </summary>
    public static class ExtendedExtention {
        private const BindingFlags TypeOfPropertise = BindingFlags.Public | BindingFlags.Instance;

        #region Empty

        /// <summary>
        ///     Checks if IsNullOrWhiteSpace.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsEmpty(this object o) {
            if (o == null) {
                return true;
            }
            if (String.IsNullOrWhiteSpace(o.ToString())) {
                return true;
            }
            return false;
        }

        #endregion

        #region Property Extensions

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties in the class.</returns>
        public static List<string> GetPropertiesNames(this object objectType) {
            //var listOfPropertise = new List<string>(40);
            if (objectType != null) {
                var properties = objectType.GetType().GetProperties(TypeOfPropertise).Select(n => n.Name).ToList();
                return properties;
                //foreach (var prop in properties) {
                //    /*object val = prop.GetValue(objectType, null);
                //    string str = "";
                //    str = String.Format("\n{0} : {1}", prop.Name.ToString(), val);
                //    output += str;
                //     * */

                //}
            }
            return null;
        }

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties in the class.</returns>
        public static PropertyInfo[] GetProperties(this object objectType) {
            //var listOfPropertise = new List<string>(40);
            if (objectType != null) {
                var properties = objectType.GetType().GetProperties(TypeOfPropertise);
                return properties;
            }
            return null;
        }

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties with values in the class.</returns>
        public static List<ObjectProperty> GetPropertiesValues(this object objectType) {
            var listOfPropertise = new List<ObjectProperty>(100);
            if (objectType != null) {
                var properties = objectType.GetType().GetProperties(TypeOfPropertise).ToList();

                if (properties != null && properties.Count > 0) {
                    foreach (var prop in properties) {
                        var property = new ObjectProperty {
                            Name = prop.Name,
                            Value = prop.GetValue(objectType, null)
                        };
                        listOfPropertise.Add(property);
                    }
                    return listOfPropertise;
                }
            }

            return null;
        }

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties with values in the class.</returns>
        public static List<ObjectProperty> GetBinary(this object objectType) {
            var listOfPropertise = new List<ObjectProperty>(100);
            if (objectType != null) {
                var properties = objectType.GetType().GetProperties(TypeOfPropertise).ToList();

                if (properties != null && properties.Count > 0) {
                    foreach (var prop in properties) {
                        var property = new ObjectProperty {
                            Name = prop.Name,
                            Value = prop.GetValue(objectType, null)
                        };
                        listOfPropertise.Add(property);
                    }
                    return listOfPropertise;
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Get current data-type name.  GetType().Name
        /// </summary>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IList<T> table) {
            return table == null || table.Count == 0;
        }

        /// <summary>
        ///     Object to binary 
        /// </summary>
        /// <param name="obj">Must be a Serializable object.</param>
        /// <returns>Returns : null if given object is null.</returns>
        public static byte[] ToBytesArray(this object obj) {
            if (obj == null) {
                return null;
            }
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        /// <summary>
        ///     Read Binary to Object
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public static object BinaryToObject(this byte[] arrBytes) {
            if (arrBytes == null || arrBytes.Length == 0) {
                return null;
            }
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }


    }
}