#region using block

using System;
using System.Linq;
using System.Reflection;

#endregion

namespace DevMvcComponent.Extensions {
    /// <summary>
    ///     Convert one entity to another if there is matching properties.
    /// </summary>
    public static class OneTypeToAnother {
        /// <summary>
        ///     Extension method for casting one type to another if there is any matching in the property name.
        ///     It returns a new object. So referencing will not work with previous object. 
        ///     Keep in mind that it is very expensive operation as 'Select' conversion.
        ///     Warning: Make matched names has exact same data type.
        /// </summary>
        /// <typeparam name="TBaseType">Your base type.</typeparam>
        /// <typeparam name="TNewType">Your return type class. Only match properties will return.</typeparam>
        /// <returns>
        ///     Obj1 { a, b, c, d }
        ///     Obj2 { b, c , d }
        ///     Obj1 obj1_ =  {a = "hello"}
        ///     Obj2 obj2_ =  obj1_.Cast<Obj1, Obj2>()
        /// </returns>
        public static TNewType Cast<TBaseType, TNewType>(this TBaseType myobj) {
            var target = typeof(TNewType);
            var x = Activator.CreateInstance(target, false); // creating a new instance of target object.
            var destination = target.GetMembers().Where(n => n.MemberType == MemberTypes.Property).ToList();
     
            var members = destination.Where(memberInfo =>
                destination.Select(c => c.Name).ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members) {
                propertyInfo = typeof(TNewType).GetProperty(memberInfo.Name);
                value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);
                propertyInfo.SetValue(x, value, null);
            }
            propertyInfo = null;
            value = null;
            GC.Collect();
            return (TNewType)x;
        }
    }
}