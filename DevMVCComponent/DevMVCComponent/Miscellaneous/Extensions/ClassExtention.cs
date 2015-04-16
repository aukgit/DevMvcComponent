using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DevMVCComponent.Miscellaneous.Extensions {
    public static class ClassExtention {

        #region Explicit Custom Types

        public class ClassProperty {
            public string PropertiesName { get; set; }
            public object PropertiesValue { get; set; }
        }

        #endregion

        #region Property Extensions

        /// <summary>
        /// Returns null if no propertise are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of propertise in the class.</returns>
        public static List<string> GetPropertiesNames(this object objectType) {
            //var listOfPropertise = new List<string>(40);
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (objectType != null) {

                var properties = objectType.GetType().GetProperties(typeOfPropertise).Select(n => n.Name).ToList();
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
        /// Returns null if no propertise are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of propertise in the class.</returns>
        public static PropertyInfo[] GetProperties(this object objectType) {
            //var listOfPropertise = new List<string>(40);
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (objectType != null) {

                var properties = objectType.GetType().GetProperties(typeOfPropertise);
                return properties;
            }
            return null;
        }

        /// <summary>
        /// Returns null if no propertise are found. 
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of propertise with values in the class.</returns>
        public static List<ClassProperty> GetPropertiesValues(this object objectType) {
            var listOfPropertise = new List<ClassProperty>(100);
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (objectType != null) {

                var properties = objectType.GetType().GetProperties(typeOfPropertise).ToList();

                if (properties != null && properties.Count > 0) {
                    foreach (var prop in properties) {
                        var property = new ClassProperty() {
                            PropertiesName = prop.Name,
                            PropertiesValue = prop.GetValue(objectType, null)
                        };
                        listOfPropertise.Add(property);
                    }
                    return listOfPropertise;
                }

            }
            
            return null;
        } 
        #endregion

        #region Empty
        /// <summary>
        /// Checks if IsNullOrWhiteSpace.
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

    }
}