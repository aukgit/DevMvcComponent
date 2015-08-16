using System;

namespace ConsoleApplication1 {
    public class DevUserValidator : Validator {


        public DevUserValidator()
            : base() {

        }
        /// <summary>
        /// Validate register code.
        /// Returns true means validation is correct.
        /// </summary>
        /// <returns></returns>
        public bool RegisterCodeValidate() {
            Console.WriteLine("RegisterCodeValidate");
            return true;
        }

        /// <summary>
        /// Validate Language
        /// </summary>
        /// <returns></returns>
        public bool LanguageValidate() {
            Console.WriteLine("LanguageValidate");

            return true;
        }


        public bool TimezoneValidate() {
            Console.WriteLine("TimezoneValidate");
            return true;
        }
        #region Overrides of Validator

        /// <summary>
        /// In this method all the  
        /// validation methods 
        /// should be added to the collection via AddValidation() method.
        /// </summary>
        public override void CollectValidation() {
            AddValidation(RegisterCodeValidate);
            AddValidation(LanguageValidate);
            AddValidation(TimezoneValidate);
        }

        #endregion
    }
}