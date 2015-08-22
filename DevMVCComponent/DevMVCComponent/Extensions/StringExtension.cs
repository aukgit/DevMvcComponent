using DevMvcComponent.DataTypeFormat;

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
        /// Returns given parameter(0) if can't convert to an int.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultParameter">Your default parameter to receive when can't convert to number.</param>
        /// <returns></returns>
        public static int ToInt(this string value, int defaultParameter = 0) {
            var integerNumber = 0;
            bool  isPossible = int.TryParse(value, out integerNumber);
            if (isPossible) {
                return integerNumber;
            }
            return defaultParameter;
        }
        /// <summary>
        /// Returns given parameter(0) if can't convert to an long.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultParameter">Your default parameter to receive when can't convert to number.</param>
        /// <returns></returns>
        public static long ToLong(this string value, long defaultParameter = 0) {
            var integerNumber = 0;
            bool isPossible = int.TryParse(value, out integerNumber);
            if (isPossible) {
                return integerNumber;
            }
            return defaultParameter;
        }

        /// <summary>
        /// Returns given parameter(0) if can't convert to an decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultParameter">Your default parameter to receive when can't convert to number.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value, decimal defaultParameter = 0) {
            decimal decimalNumber = 0;
            bool isPossible = decimal.TryParse(value, out decimalNumber);
            if (isPossible) {
                return decimalNumber;
            }
            return defaultParameter;
        }

        /// <summary>
        /// Returns given parameter(0) if can't convert to an decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultParameter">Your default parameter to receive when can't convert to number.</param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultParameter = 0) {
            double decimalNumber = 0;
            bool isPossible = double.TryParse(value, out decimalNumber);
            if (isPossible) {
                return decimalNumber;
            }
            return defaultParameter;
        }

        #region Number methods

        /// <summary>
        ///     If data type is number(int, decimal, float or single etc...) then return true.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns : true if data-type is number(int, decimal, float or single etc...)</returns>
        public static bool IsNumber(this string value) {
            return TypeChecker.IsStringNumber(value);
        }

        /// <summary>
        ///     If data type is floating point(double, decimal, float, single byte) then return true.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns : true if floating point(double, decimal, float, single byte)</returns>
        public static bool IsNonFloatingPointNumber(this string value) {
            return TypeChecker.IsStringNonFloatingPointNumber(value);
        }

        #endregion
    }
}