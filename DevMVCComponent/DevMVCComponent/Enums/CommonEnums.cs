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
        /// </summary>
        RegularMail,

        /// <summary>
        /// </summary>
        CarbonCopy,

        /// <summary>
        /// </summary>
        MailBlindCarbonCopy
    }
}