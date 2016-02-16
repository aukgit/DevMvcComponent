using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DevMvcComponent.Extensions {
    public static class CollectionExtension {
        /// <summary>
        ///     Returns a string of comma separated values(CSV)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="list"></param>
        /// <param name="selectStatement"></param>
        /// <param name="defaultValue">Default value to return when there list is null or empty.</param>
        /// <returns>Returns a string of comma separated values(CSV)</returns>
        public static string GetAsCommaSeperatedValues<TSource, TResult>(this IEnumerable<TSource> list, Func<TSource, TResult> selectStatement, string defaultValue = null) {
            //var listOfPropertise = new List<string>(40);
            if (list != null) {
                var listArray = list.Select(selectStatement).ToArray();
                if (listArray.Length == 0) {
                    return defaultValue;
                }
                var csv = string.Join(",", listArray);
                return csv;
            }
            return defaultValue;
        }
    }
}