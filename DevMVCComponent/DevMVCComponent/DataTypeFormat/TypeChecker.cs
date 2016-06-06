#region using block

using System;
using System.Text.RegularExpressions;

#endregion

namespace DevMvcComponent.DataTypeFormat {
    /// <summary>
    ///     Data type supporting class, which check whether
    ///     the data type is known(Guid, Datetime, number etc...) or a custom type.
    ///     If known then get the value for other task.
    /// </summary>
    public static class TypeChecker {
        /// <summary>
        ///     If data type is known(Guid, Datetime, number etc...) or primitive then returns true.
        ///     It only detects which data-types can be directly converted to string.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns : true if data-type is primitive type or Guid (check MSDN for more info)</returns>
        public static bool IsPrimitiveOrGuid(object o) {
            bool checkLong = o is long,
                 checkInt = o is int || o is short || o is long,
                 checkDecimal = o is float || o is decimal || o is double,
                 checkString = o is string,
                 checkGuid = o is Guid,
                 checkBool = o is bool,
                 checkDateTime = o is DateTime,
                 checkByte = o is byte;

            if (checkString || checkByte || checkLong || checkInt || checkDecimal || checkGuid || checkBool ||
                checkDateTime) {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     If data type is number(int, decimal, float or single etc...) then return true.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns : true if data-type is number(int, decimal, float or single etc...)</returns>
        public static bool IsNumber(object o) {
            bool checkLong = o is long,
                 checkInt = o is int || o is short || o is long,
                 checkSingle = o is float,
                 checkDecimal = o is float || o is decimal || o is double,
                 checkByte = o is byte;

            if (checkByte || checkLong || checkInt || checkDecimal || checkSingle) {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     If data type is number(int, decimal, float or single etc...) then return true.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns : true if data-type is number(int, decimal, float or single etc...)</returns>
        public static bool IsStringNumber(string o) {
            return Regex.IsMatch(o, @"^\d+.\d+$");
        }

        /// <summary>
        ///     If data type is non-floating point(long,int,single, byte) number then return true.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns : true if data-type is non-floating point(long,int,single, byte)</returns>
        public static bool IsIntOrLongOrByte(object o) {
            bool checkLong = o is long,
                 checkInt = o is int || o is short || o is long,
                 checkSingle = o is float,
                 checkByte = o is byte;

            if (checkByte || checkLong || checkInt || checkSingle) {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     If data type is non-floating point(long,int,single, byte) number then return true.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns : true if data-type is non-floating point(long,int,single, byte)</returns>
        public static bool IsNonFloatingPointNumber(object o) {
            return IsIntOrLongOrByte(o);
        }

        /// <summary>
        ///     If data type is non-floating point(long,int,single, byte) number then return true.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns : true if data-type is non-floating point(long,int,single, byte)</returns>
        public static bool IsStringNonFloatingPointNumber(string o) {
            return Regex.IsMatch(o, @"^\d+$");
        }

        /// <summary>
        ///     If data type is floating point(double, decimal, float, single byte) then return true.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns : true if floating point(double, decimal, float, single byte)</returns>
        public static bool IsDoubleOrDecimalOrFloat(object o) {
            return o is float || o is decimal || o is double || o is float;
        }

        /// <summary>
        ///     If data type is floating point(double, decimal, float, single byte) then return true.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Returns : true if floating point(double, decimal, float, single byte)</returns>
        public static bool IsFloatingPointNumber(object o) {
            return IsDoubleOrDecimalOrFloat(o);
        }
    }
}