#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevMVCComponent.DataTypeFormat;

#endregion

namespace DevMVCComponent.Miscellaneous.Extensions {
    /// <summary>
    ///     Get the list of classes to exporting format
    /// </summary>
    public static class ClassExtention {
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

        private const BindingFlags TypeOfPropertise = BindingFlags.Public | BindingFlags.Instance;

        #region Property Extensions

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties in the class.</returns>
        public static List<string> GetPropertiesNames(this object objectType) {
            //var listOfPropertise = new List<string>(40);
            BindingFlags typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (objectType != null) {
                List<string> properties = objectType.GetType().GetProperties(typeOfPropertise).Select(n => n.Name).ToList();
                return properties;
                //foreach (var prop in propertise) {
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
        ///     Returns null if no propertise are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of propertise in the class.</returns>
        public static PropertyInfo[] GetProperties(this object objectType) {
            //var listOfPropertise = new List<string>(40);
            BindingFlags typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (objectType != null) {
                PropertyInfo[] properties = objectType.GetType().GetProperties(typeOfPropertise);
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
            List<ObjectProperty> listOfPropertise = new List<ObjectProperty>(100);
            if (objectType != null) {
                List<PropertyInfo> properties = objectType.GetType().GetProperties(TypeOfPropertise).ToList();

                if (properties != null && properties.Count > 0) {
                    foreach (PropertyInfo prop in properties) {
                        ObjectProperty property = new ObjectProperty {
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
    }
}