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
        ///     What is the error and what is the cause of the error.
        /// </summary>
        [Required]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Solution message
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        ///     Specific class for this error label.
        ///     By default :
        ///     High : label label-danger high-priority
        ///     Medium : label label-danger medium-priority
        ///     low : label label-warning low-priority
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        ///     Where user can solve this problem.
        ///     Could be null or empty.
        /// </summary>
        public string SolutionLink { get; set; }

        /// <summary>
        ///     Title attribute of the link
        /// </summary>
        public string SolutionDisplayMessage { get; set; }

        /// <summary>
        ///     Type of the error
        /// </summary>
        [Required]
        public ErrorType Type { get; set; }
    }
}