#region using block

using DevMVCComponent.Error;
using DevMVCComponent.Mailer;
using DevMVCComponent.Processor;

#endregion

namespace DevMVCComponent {
    /// <summary>
    ///     Must setup this starter class and Config class.
    /// </summary>
    public static class Starter {
        /// <summary>
        /// </summary>
        public static Handler Error = new Handler();

        /// <summary>
        /// </summary>
        public static CookieProcessor Cookies = new CookieProcessor();

        /// <summary>
        /// </summary>
        public static CacheProcessor Caches = new CacheProcessor();

        /// <summary>
        /// </summary>
        public static ErrorCollector ErrorCollection = new ErrorCollector();

        /// <summary>
        ///     Must re-setup with appropriate developer credentials.
        /// </summary>
        public static MailConfig Mailer;

        /// <summary>
        ///     Setup the component plugin
        /// </summary>
        public static void Setup() {
        }
    }
}