namespace DevMvcComponent.Encryption {
    /// <summary>
    ///     Not yet implemented
    /// </summary>
    public class Cryptography {
        private readonly int _defaultLen = 16;
        private readonly char _seperateChar = '-';

        /// <summary>
        /// </summary>
        public Cryptography() {
            Length = _defaultLen;
            DigitLenDivision = 4;
            DivisionChar = _seperateChar;
        }

        /// <summary>
        /// </summary>
        /// <param name="len"></param>
        public Cryptography(int len) {
            Length = len;
            DigitLenDivision = 4;
            DivisionChar = _seperateChar;
        }

        public Cryptography(int len, int digitLengthForDivision) {
            Length = len;
            DigitLenDivision = digitLengthForDivision;
            DivisionChar = _seperateChar;
        }

        public Cryptography(int len, int digitLengthForDivision, char divisionChar) {
            Length = len;
            DigitLenDivision = digitLengthForDivision;
            DivisionChar = divisionChar;
        }

        private int Length { get; set; }
        private int DigitLenDivision { get; set; }
        private char DivisionChar { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string FormatCode(string code) {
            var ncode = "";
            if (code != "") {
                var len = code.Length;
                var i = 0;
                var pointDiv = DigitLenDivision;

                while ((i + pointDiv) <= len) {
                    var assumeLen = (i + pointDiv);
                    if (assumeLen >= len) {
                        ncode += code.Substring(i, pointDiv);
                        goto ReturnNow;
                    }
                    ncode += code.Substring(i, pointDiv) + DivisionChar;

                    i += pointDiv;

                    assumeLen = (i + pointDiv);
                    if (assumeLen > len) {
                        //give the last digits calculations
                        //such as if I have 32 digits and want to divide by 5 then at last we will have to pass
                        pointDiv = len%pointDiv;
                    }
                }
                ReturnNow:
                return ncode;
            }
            return "";
        }
    }
}