#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using DevMvcComponent.DataTypeFormat;

#endregion

namespace DevMvcComponent.Extensions
{
    /// <summary>
    ///     Get the list of classes to exporting format
    /// </summary>
    public static class ClassExtention
    {
        /// <summary>
        ///     Get all public instant values : BindingFlags.Public | BindingFlags.Instance
        /// </summary>
        public const BindingFlags PublicInstanceProperties = BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        ///     Get all public static and constant values : BindingFlags.Public | BindingFlags.Static |
        ///     BindingFlags.FlattenHierarchy
        /// </summary>
        public const BindingFlags ConstantProperties = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        /// <summary>
        ///     Get all public static values : BindingFlags.Public | BindingFlags.Static
        /// </summary>
        public const BindingFlags PublicStaticProperties = BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        ///     Get all private static values : BindingFlags.NonPublic | BindingFlags.Static
        /// </summary>
        public const BindingFlags PrivateStaticProperties = BindingFlags.NonPublic | BindingFlags.Static;

        /// <summary>
        ///     Save as Cookie
        /// </summary>
        public static T GetSession<T>(this T str, string name)
        {
            str = (T) HttpContext.Current.Session[name];

            return str;
        }

        /// <summary>
        ///     Save as Cookie
        /// </summary>
        public static void SaveInSession<T>(this T str, string name)
        {
            if (str == null)
            {
                HttpContext.Current.Session.Remove(name);
            }
            else
            {
                HttpContext.Current.Session[name] = str;
            }
        }

        /// <summary>
        ///     Save as Cache
        /// </summary>
        public static void SaveAsCache<T>(this T str, string name)
        {
            Mvc.Caches[name] = str;
        }

        /// <summary>
        ///     Get from cache
        /// </summary>
        public static T GetCacheValue<T>(this T str, string name)
        {
            var value = Mvc.Caches.Get(name);

            if (value != null)
            {
                return (T) value;
            }

            return default(T);
        }

        #region Empty

        /// <summary>
        ///     Checks if IsNullOrWhiteSpace.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsEmpty(this object o)
        {
            if (o == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(o.ToString()))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Property Extensions

        /// <summary>
        ///     Get values of constants
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetConstantsValues<T>(this object objectType)
        {
            if (objectType != null)
            {
                var results = objectType.GetType().GetConstants().Select(n => (T) n.GetValue(null));

                return results;
            }

            return default(IEnumerable<T>);
        }

        /// <summary>
        ///     Get constant fields from any class type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetConstants(this Type type)
        {
            var fieldInfos = type.GetFields(ConstantProperties);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
        }

        /// <summary>
        ///     Get constant fields from any class type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<ObjectProperty> GetConstantsAsObjectProperties(this Type type)
        {
            var fields = type.GetConstants();

            return fields.Select(
                n => new ObjectProperty
                {
                    Name  = n.Name,
                    Value = n.GetValue(null)
                });
        }

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties in the class.</returns>
        public static List<string> GetPropertiesNames(this object objectType)
        {
            if (objectType != null)
            {
                var properties = objectType.GetType().GetProperties(PublicInstanceProperties).Select(n => n.Name).ToList();

                return properties;
            }

            return null;
        }

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties in the class.</returns>
        public static IEnumerable<string> GetConstantNames(this object objectType)
        {
            if (objectType != null)
            {
                var properties = objectType.GetType().GetConstants().Select(n => n.Name);

                return properties;
            }

            return null;
        }

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <param name="type">
        ///     Get all public instant values : BindingFlags.Public | BindingFlags.Instance
        ///     Get all public static and constant values : BindingFlags.Public | BindingFlags.Static |
        ///     BindingFlags.FlattenHierarchy
        ///     Get all public static values : BindingFlags.Public | BindingFlags.Static
        ///     Get all private static values : BindingFlags.NonPublic | BindingFlags.Static
        /// </param>
        /// <returns>Returns the list of properties in the class.</returns>
        public static PropertyInfo[] GetPropertise(this object objectType, BindingFlags type)
        {
            if (objectType != null)
            {
                return objectType.GetType().GetProperties(type);
            }

            return null;
        }

        /// <summary>
        ///     Returns null if no FieldInfos are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <param name="type">
        ///     Get all public instant values : BindingFlags.Public | BindingFlags.Instance
        ///     Get all public static and constant values : BindingFlags.Public | BindingFlags.Static |
        ///     BindingFlags.FlattenHierarchy
        ///     Get all public static values : BindingFlags.Public | BindingFlags.Static
        ///     Get all private static values : BindingFlags.NonPublic | BindingFlags.Static
        /// </param>
        /// <returns>Returns the list of FieldInfos in the class.</returns>
        public static FieldInfo[] GetFields(this object objectType, BindingFlags type)
        {
            if (objectType != null)
            {
                return objectType.GetType().GetFields(type);
            }

            return null;
        }

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties in the class.</returns>
        public static PropertyInfo[] GetProperties(this object objectType)
        {
            //var listOfPropertise = new List<string>(40);
            if (objectType != null)
            {
                var properties = objectType.GetType().GetProperties(PublicInstanceProperties);

                return properties;
            }

            return null;
        }

        /// <summary>
        ///     Returns null if no properties are found.
        /// </summary>
        /// <param name="objectType">Type of any object/class/</param>
        /// <returns>Returns the list of properties with values in the class.</returns>
        public static List<ObjectProperty> AsObjectPropertyList(this object objectType)
        {
            var listOfPropertise = new List<ObjectProperty>(100);

            if (objectType != null)
            {
                var properties = objectType.GetType().GetProperties(PublicInstanceProperties).ToList();

                if (properties.Count > 0)
                {
                    foreach (var prop in properties)
                    {
                        var property = new ObjectProperty
                        {
                            Name  = prop.Name,
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