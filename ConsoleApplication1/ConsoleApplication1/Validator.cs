using System.Collections.Generic;

namespace ConsoleApplication1 {
    public abstract class Validator {
        protected delegate bool RunValidation();
        protected List<RunValidation> ValidationCollection;
        /// <summary>
        /// On initialization CollectValidation() is called to collect all the validations.
        /// </summary>

        protected Validator() {
  
            ValidationCollection = new List<RunValidation>(25);

            CollectValidation();
        }

        protected void AddValidation(RunValidation validation) {
            ValidationCollection.Add(validation);
        }

        protected void ClearValidation() {
            ValidationCollection.Clear();
        }

        /// <summary>
        /// In this method all the  
        /// validation methods 
        /// should be added to the 
        /// collection via AddValidation() method.
        /// Returns true means validation is correct.
        /// </summary>
        public abstract void CollectValidation();
        /// <summary>
        /// Run all the validation methods and then 
        /// set the ErrorCollector for the session.
        /// </summary>
        /// <returns>Returns true if no error exist</returns>
        public bool FinalizeValidation() {
            bool anyValidationErrorExist = false;
            foreach (var action in ValidationCollection) {
                if (!anyValidationErrorExist) {
                    anyValidationErrorExist = !action();
                }
            }
            return !anyValidationErrorExist;
        }
    }
}