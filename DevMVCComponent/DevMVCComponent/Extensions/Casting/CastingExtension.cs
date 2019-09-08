#region using block

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

namespace DevMvcComponent.Extensions.Casting
{
    /// <summary>
    ///     Convert one entity to another if there is matching properties.
    /// </summary>
    public static class CastingExtension
    {
        /// <summary>
        ///     Extension method for casting one type to another if there is any matching in the property name.
        ///     It returns a new object. So referencing will not work with previous object.
        ///     Keep in mind that it is very expensive operation as 'Select' conversion.
        ///     Warning: Make matched names has exact same data type.
        /// </summary>
        /// <typeparam name="TBaseType">Your base type.</typeparam>
        /// <typeparam name="TNewType">Your return type class. Only match properties will return.</typeparam>
        /// <param name="myobj"></param>
        /// <param name="checkTypeSafety">True : check if both types are compatible.</param>
        /// <param name="checkTypeSafetyAsAssignable">
        ///     True : use Type.IsInstanceOfType() method to check if the types are compatible.
        ///     False : check equals with each type(better performance but fails if it is compatible but now exactly same).
        /// </param>
        /// <returns>
        ///     Obj1 { a, b, c, d }
        ///     Obj2 { b, c , d }
        ///     Obj1 obj1_ =  {a = "hello"}
        ///     Obj2 obj2_ =  obj1_.Cast<Obj1, Obj2>()
        /// </returns>
        public static TNewType Cast<TBaseType, TNewType>(
            this TBaseType myobj,
            bool checkTypeSafety = true,
            bool checkTypeSafetyAsAssignable = false)
        {
            var target                = typeof(TNewType);
            var x                     = Activator.CreateInstance(target, false); // creating a new instance of target object.
            var destinationProperties = target.GetProperties(ClassExtention.PublicInstanceProperties);

            //;.Where(n => myobj.GetType().GetProperty(n.Name) != null);

            object value;

            foreach (var destpropertyInfo in destinationProperties)
            {
                var baseTypeProperty = myobj.GetType().GetProperty(destpropertyInfo.Name);

                if (baseTypeProperty != null)
                {
                    var isAssignable = true;

                    if (checkTypeSafety)
                    {
                        if (checkTypeSafetyAsAssignable)
                        {
                            isAssignable = destpropertyInfo.PropertyType.IsInstanceOfType(baseTypeProperty.PropertyType);
                        }
                        else
                        {
                            isAssignable = destpropertyInfo.PropertyType == baseTypeProperty.PropertyType;
                        }
                    }

                    if (isAssignable)
                    {
                        value = baseTypeProperty.GetValue(myobj, null);
                        destpropertyInfo.SetValue(x, value, null);
                    }
                }
            }

            value = null;
            GC.Collect();

            return (TNewType) x;
        }

        /// <summary>
        ///     Object to binary
        /// </summary>
        /// <param name="obj">Must be a Serializable object.</param>
        /// <returns>Returns : null if given object is null.</returns>
        public static byte[] ToBytesArray(this object obj)
        {
            if (obj == null)
            {
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
        public static object BinaryToObject(this byte[] arrBytes)
        {
            if (arrBytes == null ||
                arrBytes.Length == 0)
            {
                return null;
            }

            var memStream = new MemoryStream();
            var binForm   = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);

            return obj;
        }

        /// <summary>
        ///     Convert bytes to specific object
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public static T BinaryToGenericObject<T>(this byte[] arrBytes)
        {
            if (arrBytes == null ||
                arrBytes.Length == 0)
            {
                return default(T);
            }

            var memStream = new MemoryStream();
            var binForm   = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = (T) binForm.Deserialize(memStream);

            return obj;
        }
    }
}