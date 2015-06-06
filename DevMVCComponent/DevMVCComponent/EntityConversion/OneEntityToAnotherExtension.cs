using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DevMVCComponent.EntityConversion {
    /// <summary>
    /// Convert one entity to another if there is matching properties.
    /// </summary>
    public static class OneEntityToAnotherExtension {
        /// <summary>
        /// Extension method for casting one type to another if there is any matching in the property name
        /// </summary>
        /// <param name="myobj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// Obj1 { a, b, c, d }
        /// Obj2 { b, c , d }
        /// Obj2 obj2_ =  cus.Cast<Obj1>()
        /// </returns>
        public static T Cast<T>(this T myobj) {
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var destination = from src in target.GetMembers().ToList()
                    where src.MemberType == MemberTypes.Property
                    select src;
            List<MemberInfo> members = destination.Where(memberInfo => 
                destination.Select(c => c.Name).ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members) {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                propertyInfo.SetValue(x, value, null);
            }
            return (T)x;
        }  
    }
}