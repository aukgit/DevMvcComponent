using System;

namespace DevMVCComponent.Database {   

    public class DataTypeSupport {

        public static bool isSupport(object o) {
            bool checkLong = o is long;
            bool checkInt = o is int || o is Int16 || o is Int32 || o is Int64;
            bool checkDecimal = o is float || o is decimal || o is double;
            bool checkString = o is string;
            bool checkGuid = o is Guid;
            bool checkBool = o is bool;
            bool checkDateTime = o is DateTime;
            bool checkByte = o is byte || o is Byte;
            
            if (checkString || checkByte || checkLong || checkInt || checkDecimal || checkGuid || checkBool || checkDateTime)
                return true;
            else
                return false;
        }
    }
}