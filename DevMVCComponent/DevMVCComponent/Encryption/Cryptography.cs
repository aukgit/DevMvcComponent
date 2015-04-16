using System;
using System.Security.Cryptography;
using System.Text;

namespace DevMVCComponent.Encryption {
    public class Cryptography {
        private int Length { get; set; }
        private int DigitLenDivision { get; set; }

        private char DivisionChar { get; set; }

        private int defaultLen = 16;
        private char seperateChar = '-';

        public Cryptography() {
            Length = defaultLen;
            DigitLenDivision = 4;
            DivisionChar = seperateChar;
        }

        public Cryptography(int len) {
            Length = len;
            DigitLenDivision = 4;
            DivisionChar = seperateChar;
        }

        public Cryptography(int len, int digitLengthForDivision) {
            Length = len;
            DigitLenDivision = digitLengthForDivision;
            DivisionChar = seperateChar;
        }

        public Cryptography(int len, int digitLengthForDivision, char divisionChar) {
            Length = len;
            DigitLenDivision = digitLengthForDivision;
            DivisionChar = divisionChar;
        }
        public string FormatCode(string code) {
            string ncode = "";
            if (code != "") {
                int len = code.Length;
                int i = 0;
                int PointDiv = DigitLenDivision;

                while ((i + PointDiv) <= len) {
                    int assumeLen = (i + PointDiv);
                    if (assumeLen >= len) {
                        ncode += code.Substring(i, PointDiv);
                        goto ReturnNow;
                    } else {
                        ncode += code.Substring(i, PointDiv) + DivisionChar;
                    }

                    i += PointDiv;

                    assumeLen = (i + PointDiv);
                    if (assumeLen > len) {
                        //give the last digits calculations
                        //such as if I have 32 digits and want to divide by 5 then at last we will have to pass
                        PointDiv = len % PointDiv;
                    }

                }
            ReturnNow:
                return ncode;
            } else
                return "";
        }

        public class MD5Generate {

            //Generate MD5 Code
            public string GenerateCleanMD5(string input) {
                string coded;
                MD5 md5Hash = MD5.Create();
                coded = GetMd5Hash(md5Hash, input);
                return coded;
            }

            public string GetMd5Hash(MD5 md5Hash, string input) {

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF32.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++) {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }

            // Verify a hash against a string.
            public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash) {
                // Hash the input.
                string hashOfInput = GetMd5Hash(md5Hash, input);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash)) {
                    return true;
                } else {
                    return false;
                }
            }

        }
    }
}