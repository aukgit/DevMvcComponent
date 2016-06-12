#region using block

using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Caching;
using DevMvcComponent.Global;
using DevMvcComponent.Miscellaneous;

#endregion

namespace DevMvcComponent.Processor {
    /// <summary>
    ///     Default Sliding 2 Hours
    ///     Default Expiration 5 Hours
    /// </summary>
    public class CacheProcessor {
        private readonly int _defaultExpiration;

        /// <summary>
        ///     Time between inserted and last access
        /// </summary>
        private readonly int _defaultSliding;

        /// <summary>
        ///     Will be maintained by each db table as single file single text in a
        ///     specific folder.
        /// </summary>
        private CacheDependency _defaultCacheDependency;

        private string _defaultDependencyFileLocation;

        #region Operator Overloads

        /// <summary>
        ///     Sets and retrieves Cache
        /// </summary>
        /// <param name="cacheName"></param>
        public object this[string cacheName] {
            get { return Get(cacheName); }
            set {
                if (value == null) {
                    Remove(cacheName);
                }
                Set(cacheName, value);
            }
        }

        #endregion

        private void SetDefaults() {
            var rootFolder = DirectoryExtension.GetBaseOrAppDirectory();
            var dataFolder = rootFolder + "\\DataCache\\";
            if (!Directory.Exists(dataFolder)) {
                try {
                    Directory.CreateDirectory(dataFolder);
                    _defaultDependencyFileLocation = dataFolder;
                } catch (Exception) {
                    _defaultDependencyFileLocation = rootFolder;
                }
            }
        }

        #region Retrieve Cache Value

        /// <summary>
        ///     Retrieve the cache value.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Get(string name) {
            if (HttpContext.Current.Cache != null && HttpContext.Current.Cache[name] != null) {
                return HttpContext.Current.Cache[name];
            }
            return null;
        }

        /// <summary>
        ///     Retrieve the cache value as string or null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name) {
            if (HttpContext.Current.Cache != null && HttpContext.Current.Cache[name] != null) {
                return HttpContext.Current.Cache[name].ToString();
            }
            return null;
        }

        #endregion

        /*
         * Cache Insert Vs. Add
         * Insert will override existing one.
         * Add will fail if already exist one.
         * */

        #region Constructors

        /// <summary>
        ///     Default expiration on +8 hours
        /// </summary>
        /// <param name="context"></param>
        public CacheProcessor() {
            SetDefaults();
        }

        public CacheProcessor(string cacheName) {
            SetDefaults();
        }

        /// <summary>
        /// </summary>
        /// <param name="expiration">in mins</param>
        public CacheProcessor(int expiration) {
            SetDefaults();
            //override after defaults.
            _defaultExpiration = expiration;
        }

        /// <summary>
        ///     Instantiate CacheProssor
        /// </summary>
        /// <param name="expiration">in mins</param>
        /// <param name="sliding">[in mins] If data is not accessed for certain time , then it will be removed from cache.</param>
        public CacheProcessor(int expiration, int sliding) {
            SetDefaults();
            //override after defaults.
            _defaultExpiration = expiration;
            _defaultSliding = sliding;
        }

        /// <summary>
        ///     Instantiate CacheProssor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheName"></param>
        /// <param name="expiration">in mins</param>
        /// <param name="sliding">
        ///     Change Default Sliding: If data is not accessed for certain time , then it will be removed from
        ///     cache. [in mins]
        /// </param>
        public CacheProcessor(string cacheName, int expiration, int sliding) {
            SetDefaults();
            //override after defaults.
            _defaultExpiration = expiration;
            _defaultSliding = sliding;
        }

        /// <summary>
        ///     Instantiate CacheProssor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheName"></param>
        /// <param name="expiration">in mins</param>
        public CacheProcessor(string cacheName, int expiration) {
            SetDefaults();
            //override after defaults.
            _defaultExpiration = expiration;
        }

        #endregion

        #region Sets

        /// <summary>
        ///     Save cache. No Expiration and no sliding.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Set(string key, object data) {
            Set(key, data, null, null, tableName: null, priority: CacheItemPriority.Default);
        }

        /// <summary>
        ///     Save object as cache.
        /// </summary>
        /// <param name="key">Key object to look for.</param>
        /// <param name="data">Save any type of data.</param>
        /// <param name="tableName">
        ///     Name of the table to create dependencies in file (AppData\DatabaseTables\table.table). Change
        ///     the file manually if table is updated.
        /// </param>
        public void Set(string key, object data, string tableName) {
            Set(key, data, _defaultExpiration, _defaultSliding, tableName, CacheItemPriority.Default);
        }

        /// <summary>
        ///     Save object as cache.
        /// </summary>
        /// <param name="key">Key object to look for.</param>
        /// <param name="data">Save any type of data.</param>
        /// <param name="expires">[in mins]</param>
        public void Set(string key, object data, int expires) {
            Set(key, data, expires, null, null, CacheItemPriority.Default);
        }

        /// <summary>
        ///     Save object as cache.
        /// </summary>
        /// <param name="key">Key object to look for.</param>
        /// <param name="data">Save any type of data.</param>
        /// <param name="sliding">[in mins]If data is not accessed for certain time then it will be deleted from the cache memory.</param>
        /// <param name="tableName">Name of the table for dependency.</param>
        public void Set(string key, object data, int sliding, string tableName) {
            Set(key, data, null, sliding, tableName, CacheItemPriority.Default);
        }

        /// <summary>
        ///     Save object as cache.
        /// </summary>
        /// <param name="key">Key object to look for.</param>
        /// <param name="data">Save any type of data.</param>
        /// <param name="expires">[in mins]</param>
        /// <param name="sliding">[in mins]If data is not accessed for certain time then it will be deleted from the cache memory.</param>
        /// <param name="tableName">
        ///     Name of the table to create dependencies in file (AppData\DatabaseTables\table.table). Change
        ///     the file manually if table is updated.
        /// </param>
        /// <param name="priority"></param>
        public void Set(string key, object data, int? expires, int? sliding, string tableName,
            CacheItemPriority priority) {
            var cache = HttpContext.Current.Cache;

            _defaultCacheDependency = tableName != null
                                          ? new CacheDependency(_defaultDependencyFileLocation + tableName + ".table")
                                          : null;
            var expiration = Cache.NoAbsoluteExpiration;
            var cacheSliding = Cache.NoSlidingExpiration;

            if (expires != null) {
                var expires2 = (double) expires;
                expiration = DateTime.Now.AddMinutes(expires2);
            }
            if (sliding != null) {
                var sliding2 = (double) sliding;
                cacheSliding = TimeSpan.FromMinutes(sliding2);
            }

            if (data != null && key != null) {
                new Thread(() => { cache.Insert(key, data, _defaultCacheDependency, expiration, cacheSliding, priority, null); }).Start();
            }
        }

        /// <summary>
        ///     Save object as cache.
        /// </summary>
        /// <param name="key">Key object to look for.</param>
        /// <param name="data">Save any type of data.</param>
        /// <param name="expires">If put expire then don't put sliding</param>
        /// <param name="sliding">If data is not accessed for certain time then it will be deleted from the cache memory.</param>
        /// <param name="cacheDependency">New dependency cache.</param>
        /// <param name="priority"></param>
        public void Set(string key, object data, DateTime? expires, TimeSpan? sliding, CacheDependency cacheDependency,
            CacheItemPriority priority) {
            var cache = HttpContext.Current.Cache;

            var expiration = Cache.NoAbsoluteExpiration;
            var cacheSliding = Cache.NoSlidingExpiration;

            if (expires != null) {
                expiration = (DateTime) expires;
            }
            if (sliding != null) {
                cacheSliding = (TimeSpan) sliding;
            }
            if (data != null && key != null) {
                cache.Insert(key, data, cacheDependency, expiration, cacheSliding, priority, null);
            }
        }

        /// <summary>
        ///     Save object as cache.
        /// </summary>
        /// <param name="key">Key object to look for.</param>
        /// <param name="data">Save any type of data.</param>
        /// <param name="expires">If put expire then don't put sliding</param>
        /// <param name="sliding">If data is not accessed for certain time then it will be deleted from the cache memory.</param>
        /// <param name="cacheDependency">New dependency cache.</param>
        /// <param name="priority"></param>
        /// <param name="onRemoveMethod">on remove method name</param>
        public void Set(string key, object data, DateTime? expires, TimeSpan? sliding, CacheDependency cacheDependency,
            CacheItemPriority priority, CacheItemRemovedCallback onRemoveMethod) {
            var cache = HttpContext.Current.Cache;
            var expiration = Cache.NoAbsoluteExpiration;
            var cacheSliding = Cache.NoSlidingExpiration;

            if (expires != null) {
                expiration = (DateTime) expires;
            }
            if (sliding != null) {
                cacheSliding = (TimeSpan) sliding;
            }
            if (data != null && key != null) {
                cache.Insert(key, data, cacheDependency, expiration, cacheSliding, priority, onRemoveMethod);
            }
        }

        #endregion

        #region Notify File

        /// <summary>
        /// </summary>
        /// <param name="table"></param>
        public void TableStatusSetChanged(string table) {
            var path = _defaultDependencyFileLocation + table + ".table";
            File.WriteAllText(path, Constants.Changed);
        }

        /// <summary>
        /// </summary>
        /// <param name="table"></param>
        public void TableStatusSetUnChanged(string table) {
            try {
                var path = _defaultDependencyFileLocation + table + ".table";
                File.WriteAllText(path, Constants.UnChanged);
            } catch (Exception ex) {
                Mvc.Error.HandleBy(ex);
            }
        }

        /// <summary>
        ///     No update in the table since the cache.
        ///     True : No-Update, False: Updated.
        /// </summary>
        /// <param name="table"></param>
        /// <returns>True : No-Update, False: Updated.</returns>
        public bool TableStatusCheck(string table) {
            var path = _defaultDependencyFileLocation + table + ".table";
            if (File.Exists(path)) {
                var readFromText = File.ReadAllText(path);
                if (readFromText.StartsWith(Constants.UnChanged)) {
                    return true; // no update
                }
            }

            return false; // updated
        }

        #endregion

        #region Remove Cache

        public void Remove(string name) {
            var cache = HttpContext.Current.Cache;
            if (cache[name] != null) {
                cache.Remove(name);
            }
        }

        public void RemoveAllFromCache() {
            foreach (DictionaryEntry entry in HttpContext.Current.Cache) {
                HttpContext.Current.Cache.Remove((string) entry.Key);
            }
        }

        #endregion
    }
}