#region using block

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevMVCComponent.DataTypeFormat;

#endregion

namespace DevMVCComponent.ObjectToArray {
    public class ObjectToArrary {
        public static List<ObjectProperty> Get(object Class) {
            if (Class != null) {
                var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
                var propertise =
                    Class.GetType()
                        .GetProperties(typeOfPropertise)
                        .Where(p => /* p.Name != "EntityKey" &&*/ p.Name != "EntityState");

                var list = new List<ObjectProperty>(propertise.Count());
                foreach (var prop in propertise) {
                    var val = prop.GetValue(Class, null);
                    var propertyName = prop.Name;
                    var obj = new ObjectProperty {
                        Name = propertyName,
                        Value = val
                    };
                    list.Add(obj);
                }
                return list;
            }
            return null;
        }
    }
}