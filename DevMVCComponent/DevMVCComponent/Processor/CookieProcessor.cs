#region using block

using System;
using System.Linq;
using System.Collections.Specialized;
using System.Web;

#endregion

namespace DevMvcComponent.Processor {
    /// <summary>
    ///     Set cookies in response
    ///     Retrieve cookies from request.
    /// </summary>
    public class CookieProcessor {
        #region Operator Overloads

        /// <summary>
        ///     Sets and retrieves Cookie as string only.
        ///     Setting null will remove the cookie.
        /// </summary>
        /// <param name="cookieName"></param>
        public string this[string cookieName] {
            get { return Get(cookieName); }
            set {
                if (value == null) {
                    Remove(cookieName);
                }
                Set(cookieName, value);
            }
        }

        #endregion
        /// <summary>
        /// Is cookie exist in the dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name) {
            return HttpContext.Current.Request.Cookies.AllKeys.Contains(name);
        }

        #region Remove Cookies

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name) {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null) {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Request.Cookies.Remove(name);
            }
            cookie = HttpContext.Current.Response.Cookies[name];
            if (cookie != null) {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Remove(name);
            }
        }

        #endregion

        #region Get Cookie -> Same as Reading

        /// <summary>
        ///     Get Default cookie string value.
        /// </summary>
        /// <returns>GetDefault cookie string value.</returns>
        public string Get(string cookieName) {
            return ReadString(cookieName);
        }

        #endregion

        // Cookies add will add duplicate cookies.
        // Cookies set will only add unique cookies.

        #region Save Cookies

        /// <summary>
        ///     Save a single object as cookie.
        ///     Save in Response.
        /// </summary>
        /// <param name="value">Pass the object</param>
        /// <param name="cookieName">Cookie name , pass null if constructor CookieName is valid.</param>
        /// <param name="checkBeforeExist">True: Don't save if already exist. </param>
        /// <param name="expiration"></param>
        public void Save(string value, string cookieName, bool checkBeforeExist = true, DateTime? expiration = null) {
            expiration = expiration ?? DateTime.Now.AddHours(5);
            HttpCookie httpCookie = null;

            httpCookie = new HttpCookie(cookieName) {
                Expires = expiration.Value
            };
            httpCookie.Value = value;
            //HttpContext.Current.Response.Cookies.Remove(cookieName);
            //HttpContext.Current.Request.Cookies.Remove(cookieName);
            var cookies = HttpContext.Current.Response.Cookies;
            if (Exists(cookieName)) {
                cookies.Set(httpCookie);
            } else {
                cookies.Add(httpCookie);
            }
            //HttpContext.Current.Request.Cookies.Set(httpCookie);
        }

        #endregion

        #region Constructor

        //public CookieProcessor(ControllerContext context) {
        //    this.controllerContext = context;
        //    this.httpContext = this.controllerContext.HttpContext;
        //}

        #endregion

        #region Set Cookie -> Same as Saving

        /// <summary>
        ///     Save cookie. +5 hours expiration.
        /// </summary>
        /// <returns>GetDefault cookie string value.</returns>
        public void Set(string str, string cookieName) {
            Save(str, cookieName);
        }

        /// <summary>
        ///     Save cookie. +5 hours expiration.
        /// </summary>
        /// <returns>GetDefault cookie string value.</returns>
        public void Set(string str, string cookieName, DateTime expires) {
            Save(str, cookieName, true, expires);
        }

        #endregion

        #region Read Cookie

        /// <summary>
        ///     Read cookie from request.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns>Return object or null.</returns>
        public NameValueCollection Read(string cookieName) {
            var httpCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (httpCookie != null) {
                if (httpCookie.Values.Count > 1) {
                    // complex type not a value.
                    return httpCookie.Values;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        ///     Read cookie from request.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="defaultValue">Default value if not found</param>
        /// <returns>Returns string or null.</returns>
        public string ReadString(string cookieName, string defaultValue = null) {
            var httpCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (httpCookie != null) {
                if (httpCookie.Values.Count == 1) {
                    // complex type not a value.
                    return httpCookie.Value;
                }
                if (httpCookie.Values.Count == 0) {
                    httpCookie = HttpContext.Current.Response.Cookies[cookieName];
                    if (httpCookie != null && httpCookie.Values.Count == 1) {
                        return httpCookie.Value;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///     Read cookie from request.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns>Returns Boolean.</returns>
        public bool ReadBool(string cookieName) {
            var n = ReadString(cookieName);
            if (!string.IsNullOrWhiteSpace(n)) {
                var res = false;
                if (bool.TryParse(n, out res)) {
                    return res;
                }
            }
            return false;
        }

        /// <summary>
        ///     Read cookie from request.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="defaultValue">Default value if can't parse</param>
        /// <returns>Returns decimal.</returns>
        public decimal? ReadDecimal(string cookieName, decimal? defaultValue = 0) {
            var n = ReadString(cookieName);
            decimal res = 0;
            if (!string.IsNullOrWhiteSpace(n)) {
                if (decimal.TryParse(n, out res)) {
                    return res;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///     Read cookie from request.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="defaultValue">Default value if can't parse</param>
        /// <returns>Returns long</returns>
        public long? ReadLong(string cookieName, long? defaultValue = 0) {
            var n = ReadString(cookieName);
            long res = 0;
            if (!string.IsNullOrWhiteSpace(n)) {
                if (long.TryParse(n, out res)) {
                    return res;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///     Read cookie from request.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="defaultValue">Default value if can't parse</param>
        /// <returns>Returns int.</returns>
        public int? ReadInt(string cookieName, int? defaultValue = 0) {
            var n = ReadString(cookieName);
            var res = 0;
            if (!string.IsNullOrWhiteSpace(n)) {
                if (int.TryParse(n, out res)) {
                    return res;
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///     Read cookie from request.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="defaultValue">Default value if can't parse</param>
        /// <returns>Return Date time or null.</returns>
        public DateTime? ReadDateTime(string cookieName, DateTime? defaultValue = null) {
            var n = ReadString(cookieName);
            DateTime res;
            if (!string.IsNullOrWhiteSpace(n)) {
                if (DateTime.TryParse(n, out res)) {
                    return res;
                }
            }
            return defaultValue;
        }

        #endregion
    }
}