using DevMVCComponent.Cache;
using DevMVCComponent.Cookie;
using DevMVCComponent.Error;
using DevMVCComponent.Mailers;
using DevMVCComponent.UserError;

namespace DevMVCComponent {
    /// <summary>
    /// Must setup this starter class and Config class.
    /// </summary>
    public static class Starter {
        public static Handle HanldeError = new Handle();
        public static CookieProcessor Cookies = new CookieProcessor();
        public static CacheProcessor Caches = new CacheProcessor();
        public static ErrorCollector ErrorCollection = new ErrorCollector();
        /// <summary>
        /// Must resetup with appropriate developer credentials.
        /// </summary>
        public static MailConfig Mailer;
     
    }
}