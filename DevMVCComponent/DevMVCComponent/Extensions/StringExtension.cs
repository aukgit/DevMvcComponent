using System;
using System.IO;
using System.Text;
using DevMvcComponent.DataTypeFormat;

namespace DevMvcComponent.Extensions {
    /// <summary>
    ///     String extensions
    /// </summary>
    public static class StringExtension {
        /// <summary>
        ///     Concatenation with other strings if first one is not null.
        /// </summary>
        /// <param name="currentString">If this one is null then no other will concat.</param>
        /// <param name="otherStrings">Other string to concat if currentString is not null.</param>
        /// <returns>Returns : Empty string ("") if current string is null.</returns>
        public static string DependingStringConcat(this string currentString, params string[] otherStrings) {
            if (currentString != null) {
                return string.Concat(otherStrings);
            }
            return string.Empty;
        }
        /// <summary>
        /// Get default string if the str is empty (null or "")
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns>Returns default value if str is empty (null or "")</returns>
        public static string GetDefaultIfEmpty(this string str, string defaultValue = "") {
            if (defaultValue.IsNullOrEmpty()) {
                str = defaultValue;
            }
            return str;
        }

        /// <summary>
        ///     Creates and returns a new FileInfo if string points to a valid file path or else null.
        /// </summary>
        /// <returns>Returns : FileInfo if file path is valid.</returns>
        public static FileInfo AsFileInfo(this string filePath) {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) {
                return new FileInfo(filePath);
            }
            return null;
        }

        /// <summary>
        ///    Checks and returns if a file exist at this path.
        /// </summary>
        /// <returns>Returns true if file path is valid.</returns>
        public static bool IsFileExists(this string filePath) {
            return !string.IsNullOrEmpty(filePath) && File.Exists(filePath);
        }

        /// <summary>
        ///     Save as Cookie
        /// </summary>
        public static void SaveAsCookie(this string str, string name, DateTime? expires = null) {
            if (!expires.HasValue) {
                Mvc.Cookies.Set(str, name);
            } else {
                Mvc.Cookies.Set(str, name, expires.Value);
            }
        }

        /// <summary>
        ///     Get from cache
        /// </summary>
        public static string GetCookieValue(this string str, string name, string defaultValue = "") {
            return Mvc.Cookies.ReadString(name, defaultValue);
        }

        /// <summary>
        ///     Only create string builder if the current string is not null.
        ///     (By default capacity will be set by the string length + 50)
        ///     returns null if the string is null.
        /// </summary>
        /// <param name="currentString">If this one is null then no other will concat.</param>
        /// <param name="additionalCapacityToLength">Given value will be addition with string length</param>
        /// <returns>Returns : string builder or null.</returns>
        public static StringBuilder GetStringBuilder(this string currentString, int additionalCapacityToLength = -1) {
            if (currentString != null) {
                var capacity = additionalCapacityToLength == -1 ? currentString.Length + 50 : currentString.Length + additionalCapacityToLength;
                var sb = new StringBuilder(capacity);
                sb.Append(currentString);
                return sb;
            }
            return null;
        }

        /// <summary>
        ///     Get ToCharArray with added length.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="addedLength">0 means calling the same function of ToCharArray</param>
        /// <returns></returns>
        public static char[] ToCharArrayPadded(this string str, int addedLength = 0) {
            if (str != null) {
                var arr = new char[str.Length + addedLength];
                for (var i = 0; i < str.Length; i++) {
                    arr[i] = str[i];
                }
                return arr;
            }
            return null;
        }

        /// <summary>
        ///     Split the string based on csv and returns an array or null if the string is null.
        /// </summary>
        /// <param name="currentString">If this one is null then no other will concat.</param>
        /// <param name="spliter"></param>
        /// <param name="options"></param>
        /// <returns>Returns : array or null.</returns>
        public static string[] GetCsvAsArray(this string currentString, string spliter = ",", StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) {
            if (currentString != null) {
                var spliterArr = new[] { spliter };
                return currentString.Split(spliterArr, options);
            }
            return null;
        }

        /// <summary>
        ///     Efficient method: starts comparing string from backward. if found then stop and skip escapseIndexUpto will be the
        ///     ending loop num.
        ///     Returns true/false if compareString is matched with the string last part.
        ///     For example : "Hello World.js".IsStringMatchfromLast(".js") ; returns true.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparingString">value that should matched with last indexes</param>
        /// <param name="escapseIndexUpto">
        ///     if any value given upto that length the string will not be compared, however if any given then the algorithm will
        ///     compare upto this value.
        ///     For example :
        ///     "1234".IsStringMatchfromLast("4", 4); // returns false.
        ///     "1234".IsStringMatchfromLast("4", 3); // returns true . loop only run for last single char and escapes first 3
        ///     chars.
        ///     "1234".IsStringMatchfromLast("4", 2); // returns true . loop only run for last 2 chars.
        ///     "1234".IsStringMatchfromLast("34", 2); // returns true . loop only run for last 2 chars.
        ///     "1234".IsStringMatchfromLast("234", 2); // returns false . loop only run for last 2 chars.
        /// </param>
        /// <returns>
        ///     Returns true/false if compareString is matched with the string last part.
        ///     For example : "Hello World.js".IsStringMatchfromLast(".js") ; returns true.
        /// </returns>
        public static bool IsMatchAtLast(this string value, string comparingString, int escapseIndexUpto = -1) {
            var compareLen = comparingString.Length - 1;
            if (escapseIndexUpto == -1) {
                escapseIndexUpto = 0;
            }
            bool result = true,
                loopRun = false;
            for (int i = value.Length - 1, k = 0; i >= escapseIndexUpto; i--, k++) {
                var index = compareLen - k;
                loopRun = true;
                if (index < 0) {
                    return result;
                }
                var isCharNotSame = value[i] != comparingString[index];
                if (isCharNotSame) {
                    var passedLength = k;
                    var isComparedLengthMatched = passedLength == compareLen + 1;
                    if (isComparedLengthMatched) {
                        return true;
                    }
                    result = false;
                }
            }
            return result && loopRun;
        }

        #region String Manipulation

        /// <summary>
        ///     First char upper case and others are in lowercase
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstCharUppercase(this string value) {
            //
            // Uppercase the first letter in the string.
            //
            if (value.Length > 0) {
                var array = value.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                return new string(array);
            }
            return value;
        }

        #endregion

        #region Striung null or empty check

        /// <summary>
        ///     Is the string null
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Returns : true if string is null.</returns>
        public static bool IsNull(this string str) {
            return str == null;
        }

        /// <summary>
        ///     Is the string null or empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Returns : true if string is null or empty.</returns>
        public static bool IsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        ///     Is the string null or empty or whitespace
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Returns : true if string is null or empty or whitespace.</returns>
        public static bool IsNullOrWhiteSpace(this string str) {
            return string.IsNullOrWhiteSpace(str);
        }

        #endregion

        #region String truncating

        /// <summary>
        ///     Split the string into pieces.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">If string len is less then return whole string. Null means whole len.</param>
        /// <returns></returns>
        public static string GetStringCutOff(this string str, int? length) {
            if (string.IsNullOrEmpty(str)) {
                return string.Empty;
            }
            if (length == null) {
                length = str.Length;
            }
            if (str.Length <= length) {
                return str;
            }
            return str.Substring(0, (int) length);
        }

        /// <summary>
        ///     Only cut of the string if necessary.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="starting">If previous mid was on 100 , start from 100</param>
        /// <param name="length">-1 means whole return last length.</param>
        /// <returns></returns>
        public static string GetStringCutOff(this string str, int starting, int length) {
            if (string.IsNullOrEmpty(str)) {
                return string.Empty;
            }
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

        #endregion

        #region Number conversion

        /// <summary>
        ///     Returns given parameter(0) if can't convert to an int.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultParameter">Your default parameter to receive when can't convert to number.</param>
        /// <returns></returns>
        public static int ToInt(this string value, int defaultParameter = 0) {
            var integerNumber = 0;
            var isPossible = int.TryParse(value, out integerNumber);
            if (isPossible) {
                return integerNumber;
            }
            return defaultParameter;
        }

        /// <summary>
        ///     Returns given parameter(0) if can't convert to an long.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultParameter">Your default parameter to receive when can't convert to number.</param>
        /// <returns></returns>
        public static long ToLong(this string value, long defaultParameter = 0) {
            var integerNumber = 0;
            var isPossible = int.TryParse(value, out integerNumber);
            if (isPossible) {
                return integerNumber;
            }
            return defaultParameter;
        }

        /// <summary>
        ///     Returns given parameter(0) if can't convert to an decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultParameter">Your default parameter to receive when can't convert to number.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value, decimal defaultParameter = 0) {
            decimal decimalNumber = 0;
            var isPossible = decimal.TryParse(value, out decimalNumber);
            if (isPossible) {
                return decimalNumber;
            }
            return defaultParameter;
        }

        /// <summary>
        ///     Returns given parameter(0) if can't convert to an decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultParameter">Your default parameter to receive when can't convert to number.</param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultParameter = 0) {
            double decimalNumber = 0;
            var isPossible = double.TryParse(value, out decimalNumber);
            if (isPossible) {
                return decimalNumber;
            }
            return defaultParameter;
        }

        #endregion

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

        /// <summary>
        ///     If data type is floating point(double, decimal, float, single byte) then return true.
        ///     Same as IsNonFloatingPointNumber()
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns : true if floating point(double, decimal, float, single byte)</returns>
        public static bool IsIntegerNumber(this string value) {
            return TypeChecker.IsStringNonFloatingPointNumber(value);
        }

        #endregion
    }
}