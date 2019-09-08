#region using block

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevMvcComponent.DataTypeFormat;

#endregion

namespace DevMvcComponent.EntityConversion
{
    /// <summary>
    ///     Generates any class to ObjectProperty data type
    /// </summary>
    public static class ObjectToArrary
    {
        private const BindingFlags TypeOfPropertise = BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        ///     Generates any class to ObjectProperty data type
        /// </summary>
        /// <param name="Class">Give any class object to retrieve it's property and values.</param>
        /// <returns></returns>
        public static List<ObjectProperty> Get(object Class)
        {
            if (Class != null)
            {
                var propertise =
                    Class.GetType()
                         .GetProperties(TypeOfPropertise)
                         .Where(p => /* p.Name != "EntityKey" &&*/ p.Name != "EntityState")
                         .ToList();

                var list = new List<ObjectProperty>(propertise.Count);

                foreach (var prop in propertise)
                {
                    var val          = prop.GetValue(Class, null);
                    var propertyName = prop.Name;

                    var obj = new ObjectProperty
                    {
                        Name  = propertyName,
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