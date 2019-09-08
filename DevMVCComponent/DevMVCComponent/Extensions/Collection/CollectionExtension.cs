using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevMvcComponent.EntityConversion;

namespace DevMvcComponent.Extensions.Collection
{
    /// <summary>
    ///     Collection , IEnumerable extensions
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        ///     Checks items == null || !items.Any()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items) => items == null || !items.Any();

        /// <summary>
        ///     Convert the whole list elements to html table string.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableCaption"></param>
        /// <returns></returns>
        public static string AsHtmlTableString(this IList<object> list, string tableCaption = "") => EntityToString.GetHtmlTableOfEntities(list, tableCaption);

        /// <summary>
        ///     Only create string builder if the current list is not null.
        ///     (By default capacity will be set by the list length + 50)
        ///     returns null if the list is null.
        ///     Warning : list values will NOT be inserted into the string builder automatically.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="additionalCapacityToLength">Given value will be addition with string length</param>
        /// <returns>Returns : string builder or null.</returns>
        public static StringBuilder AsStringBuilder<T>(this IEnumerable<T> list, int additionalCapacityToLength = -1)
        {
            if (list != null)
            {
                var enumerable = list as T[] ?? list.ToArray();
                var count      = enumerable.Length;
                var capacity   = additionalCapacityToLength == -1 ? count + 50 : count + additionalCapacityToLength;
                var sb         = new StringBuilder(capacity);

                return sb;
            }

            return null;
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
            string defaultValue = null)
        {
            //var listOfPropertise = new List<string>(40);
            if (list != null)
            {
                var listArray = list.Select(selectStatement).ToArray();

                if (listArray.Length == 0)
                {
                    return defaultValue;
                }

                var csv = string.Join(seperator, listArray);

                return csv;
            }

            return defaultValue;
        }
    }
}