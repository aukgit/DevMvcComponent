using System;

namespace DevMvcComponent.Extensions {
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtension {
        /// <summary>
        ///     Split the string into pieces.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">If string len is less then return whole string. Null means whole len.</param>
        /// <returns></returns>
        public static string GetStringCutOff(this string str, int? length) {
            if (string.IsNullOrEmpty(str))
                return "";
            if (length == null) {
                length = str.Length;
            }
            if (str.Length <= length) {
                return str;
            }
            return str.Substring(0, (int) length);
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="starting">If previous mid was on 100 , start from 100</param>
        /// <param name="length">-1 means whole return last len.</param>
        /// <returns></returns>
        public static string GetStringCutOff(this string str, int starting, int length) {
            if (string.IsNullOrEmpty(str))
                return "";
            if (length == -1) {
                length = str.Length;
            }
            if (str.Length < starting) {
                return "";
            }
            if (str.Length <= length) {
                length = str.Length;
            }
            length = length - starting;

            return str.Substring(starting, length);
        }

        /// <summary>
        /// First char upper case and others are in lowercase
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstCharUppercase(this string value) {
            //
            // Uppercase the first letter in the string.
            //
            if (value.Length > 0) {
                char[] array = value.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                return new string(array);
            }
            return value;
        }
        /// <summary>
        /// Returns 0 if can't convert to an int.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value) {
            var integerNumber = 0;
            bool  isPossible = int.TryParse(value, out integerNumber);
            if (isPossible) {
                return integerNumber;
            }
            return 0;
        }

        /// <summary>
        /// Returns 0 if can't convert to an decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value) {
            decimal decimalNumber = 0;
            bool isPossible = decimal.TryParse(value, out decimalNumber);
            if (isPossible) {
                return decimalNumber;
            }
            return 0;
        }

        /// <summary>
        /// Returns 0 if can't convert to an decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this string value) {
            double decimalNumber = 0;
            bool isPossible = double.TryParse(value, out decimalNumber);
            if (isPossible) {
                return decimalNumber;
            }
            return 0;
        }
    }
}