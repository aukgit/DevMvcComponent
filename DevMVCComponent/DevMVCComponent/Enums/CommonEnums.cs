namespace DevMvcComponent.Enums {
    /// <summary>
    /// </summary>
    public enum ErrorType {
        /// <summary>
        ///     High priority error.
        /// </summary>
        High,

        /// <summary>
        ///     Medium priority error.
        /// </summary>
        Medium,

        /// <summary>
        ///     Low priority error.
        /// </summary>
        Low
    }

    /// <summary>
    /// </summary>
    public enum DateTimeFormatType {
        /// <summary>
        /// Date format
        /// </summary>
        Date,

        /// <summary>
        /// </summary>
        Time,

        /// <summary>
        /// </summary>
        DateTimeSimple,

        /// <summary>
        /// </summary>
        DateTimeFull,

        /// <summary>
        /// </summary>
        DateTimeShort,

        /// <summary>
        /// </summary>
        DateTimeCustom
    }

    /// <summary>
    /// </summary>
    public enum MailingType {
        /// <summary>
        /// Regular email category
        /// </summary>
        RegularMail,

        /// <summary>
        /// Carbon copy email category
        /// </summary>
        CarbonCopy,

        /// <summary>
        /// Blind carbon copy category.
        /// </summary>
        MailBlindCarbonCopy
    }
}