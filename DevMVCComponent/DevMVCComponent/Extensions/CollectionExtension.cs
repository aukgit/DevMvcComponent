using System;
using System.Collections.Generic;
using System.Linq;

namespace DevMvcComponent.Extensions {
    /// <summary>
    ///     Collection , IEnumerable extensions
    /// </summary>
    public static class CollectionExtension {
        /// <summary>
        ///     Returns a string of comma separated values(CSV)
        ///     Same method AsCsv
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="list"></param>
        /// <param name="seperator">separator default coma (,)</param>
        /// <param name="selectStatement"></param>
        /// <param name="defaultValue">Default value to return when there list is null or empty.</param>
        /// <returns>Returns a string of comma separated values(CSV)</returns>
        public static string GetAsCommaSeperatedValues<TSource, TResult>(
            this IEnumerable<TSource> list,
            Func<TSource, TResult> selectStatement,
            string seperator = ",",
            string defaultValue = null) {
            return AsCsv(list, selectStatement, seperator, defaultValue);
        }

        /// <summary>
        ///     Returns a string of comma separated values(CSV)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="list"></param>
        /// <param name="selectStatement"></param>
        /// <param name="seperator">separator default coma (,)</param>
        /// <param name="defaultValue">Default value to return when there list is null or empty.</param>
        /// <returns>Returns a string of comma separated values(CSV)</returns>
        public static string AsCsv<TSource, TResult>(
            this IEnumerable<TSource> list,
            Func<TSource, TResult> selectStatement,
            string seperator = ",",
            string defaultValue = null) {
            //var listOfPropertise = new List<string>(40);
            if (list != null) {
                var listArray = list.Select(selectStatement).ToArray();
                if (listArray.Length == 0) {
                    return defaultValue;
                }
                var csv = string.Join(seperator, listArray);
                return csv;
            }
            return defaultValue;
        }
    }
}