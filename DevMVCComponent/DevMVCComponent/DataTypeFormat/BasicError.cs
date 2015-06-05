#region using block

using System.ComponentModel.DataAnnotations;
using DevMVCComponent.Enums;

#endregion

namespace DevMVCComponent.DataTypeFormat {
    /// <summary>
    /// </summary>
    public class BasicError {
        /// <summary>
        /// </summary>
        [Required]
        public short OrderId { get; set; }

        /// <summary>
        /// </summary>
        [Required]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// </summary>
        public string LinkTitle { get; set; }

        [Required]
        public ErrorType Type { get; set; }
    }
}