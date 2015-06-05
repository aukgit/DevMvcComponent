#region using block

using System;

#endregion

namespace DevMVCComponent.Database {
    public class DataTypeSupport {
        public static bool IsSupport(object o) {
            var checkLong = o is long;
            var checkInt = o is int || o is Int16 || o is Int32 || o is Int64;
            var checkDecimal = o is float || o is decimal || o is double;
            var checkString = o is string;
            var checkGuid = o is Guid;
            var checkBool = o is bool;
            var checkDateTime = o is DateTime;
            var checkByte = o is byte || o is Byte;

            if (checkString || checkByte || checkLong || checkInt || checkDecimal || checkGuid || checkBool ||
                checkDateTime)
                return true;
            return false;
        }
    }
}