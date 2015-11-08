#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace DevMvcComponent.Pagination {
    /// <summary>
    ///     Generates pagination
    /// </summary>
    public static class Pagination {
        private static long _pageItems = 50;

        /// <summary>
        ///     Total number of items in a page.
        /// </summary>
        public static long PageItems {
            get { return _pageItems; }
            set { _pageItems = value; }
        }
        /// <summary>
        ///     Get pagination data based on the page number with cached pages count.
        /// </summary>
        /// <param name="entities">Send Entities as List</param>
        /// <param name="pages">
        ///     Send a ref of integer to get the pages number. It will be generated from method. Use it to generate
        ///     pages. It indicated how many pages exist
        /// </param>
        /// <param name="page">Based on the page number it returns data. </param>
        /// <param name="items">
        ///     How many items should a page contain? (Default is 30 defined in the PageItems Property of the
        ///     class)
        /// </param>
        /// <param name="cacheName">Create cache by this exact same name. If null then no cache created.</param>
        /// <param name="retrivePagesExist">
        ///     If false then no count query will be executed. If yes then count query will only
        ///     generated if needed and not exist in the cache.
        /// </param>
        /// <returns>IEnumerable data based on the page number.</returns>
        public static IEnumerable<T> GetPageData<T>(this IList<T> entities, string cacheName, ref int? pages,
            long? page = 1, long? items = -1, bool retrivePagesExist = true) {
            if (page == null || page <= 0) {
                page = 1;
            }

            if (items == null || items == -1)
                items = PageItems;

            var take = (int)items;
            var skip = (int)page * take - take; //5 * 10 - 10
            //var hashCode = entities.GetHashCode();
            var cachePages = pages ?? -1;

            if (!string.IsNullOrEmpty(cacheName)) {
                var cachePagesString = Mvc.Caches.GetString(cacheName);
                if (cachePagesString != null) {
                    cachePages = int.Parse(cachePagesString);
                    pages = cachePages;
                }
            }
            if (cachePages < 0 && retrivePagesExist) {
                decimal pagesExist = 1;
                pagesExist = entities.Count() / (decimal)items;
                pages = (int)Math.Ceiling(pagesExist);
                Mvc.Caches.Set(cacheName, pages);
            }

            return entities.Skip(skip).Take(take);
        }

        /// <summary>
        ///     Get pagination data based on the page number with cached pages count.
        /// </summary>
        /// <param name="entities">Send Entities as IList</param>
        /// <param name="pageInfo">
        ///     Send a ref of integer to get the pages number. It will be generated from method. Use it to
        ///     generate pages. It indicated how many pages exist
        /// </param>
        /// <param name="cacheName">Create cache by this exact same name. If null then no cache created.</param>
        /// <param name="retrivePagesExist">
        ///     If false then no count query will be executed. If yes then count query will only
        ///     generated if needed and not exist in the cache.
        /// </param>
        /// <returns>IEnumerable data based on the page number.</returns>
        public static IEnumerable<T> GetPageData<T>(this IList<T> entities, PaginationInfo pageInfo,
            string cacheName = null, bool retrivePagesExist = true) {
            if (pageInfo.PageNumber == null || pageInfo.PageNumber <= 0) {
                pageInfo.PageNumber = 1;
            }

            if (pageInfo.ItemsInPage == null || pageInfo.ItemsInPage == -1)
                pageInfo.ItemsInPage = PageItems;

            var take = (int)pageInfo.ItemsInPage;
            var skip = (int)pageInfo.PageNumber * take - take; //5 * 10 - 10
            //var hashCode = entities.GetHashCode();
            var cachePages = pageInfo.PagesExists == null ? -1 : (int)pageInfo.PagesExists;
            var saveCache = false;
            if (!string.IsNullOrEmpty(cacheName)) {
                var cachePagesString = Mvc.Caches.GetString(cacheName);
                if (cachePagesString != null) {
                    cachePages = int.Parse(cachePagesString);
                    pageInfo.PagesExists = cachePages;
                }
                saveCache = true;
            }
            if (cachePages < 0 && retrivePagesExist) {
                decimal pagesExist = 1;
                pagesExist = entities.Count() / (decimal)pageInfo.ItemsInPage;
                pageInfo.PagesExists = (int)Math.Ceiling(pagesExist);
            } else {
                pageInfo.PagesExists = cachePages;
            }
            if (saveCache) {
                Mvc.Caches.Set(cacheName, pageInfo.PagesExists);
            }
            return entities.Skip(skip).Take(take);
        }
        /// <summary>
        ///     Get pagination data based on the page number with cached pages count.
        /// </summary>
        /// <param name="entities">Sent Entities as IQueryable</param>
        /// <param name="pages">
        ///     Send a ref of integer to get the pages number. It will be generated from method. Use it to generate
        ///     pages. It indicated how many pages exist
        /// </param>
        /// <param name="page">Based on the page number it returns data. </param>
        /// <param name="items">
        ///     How many items should a page contain? (Default is 30 defined in the PageItems Property of the
        ///     class)
        /// </param>
        /// <param name="cacheName">Create cache by this exact same name. If null then no cache created.</param>
        /// <param name="retrivePagesExist">
        ///     If false then no count query will be executed. If yes then count query will only
        ///     generated if needed and not exist in the cache.
        /// </param>
        /// <returns>IQueryable data based on the page number.</returns>
        public static IQueryable<T> GetPageData<T>(this IQueryable<T> entities, string cacheName, ref int? pages,
            long? page = 1, long? items = -1, bool retrivePagesExist = true) {
            if (page == null || page <= 0) {
                page = 1;
            }

            if (items == null || items == -1)
                items = PageItems;

            var take = (int)items;
            var skip = (int)page * take - take; //5 * 10 - 10
            //var hashCode = entities.GetHashCode();
            var cachePages = pages == null ? -1 : (int)pages;

            if (!string.IsNullOrEmpty(cacheName)) {
                string cachePagesString = Mvc.Caches.GetString(cacheName);
                if (cachePagesString != null) {
                    cachePages = int.Parse(cachePagesString);
                    pages = cachePages;
                }
            }
            if (cachePages < 0 && retrivePagesExist) {
                decimal pagesExist = 1;
                pagesExist = entities.Count() / (decimal)items;
                pages = (int)Math.Ceiling(pagesExist);
                Mvc.Caches.Set(cacheName, pages);
            }

            return entities.Skip(skip).Take(take);
        }

        /// <summary>
        ///     Get pagination data based on the page number with cached pages count.
        /// </summary>
        /// <param name="entities">Sent Entities as IQueryable</param>
        /// <param name="pageInfo">
        ///     Send a ref of integer to get the pages number. It will be generated from method. Use it to
        ///     generate pages. It indicated how many pages exist
        /// </param>
        /// <param name="cacheName">Create cache by this exact same name. If null then no cache created.</param>
        /// <param name="retrivePagesExist">
        ///     If false then no count query will be executed. If yes then count query will only
        ///     generated if needed and not exist in the cache.
        /// </param>
        /// <returns>IQueryable data based on the page number.</returns>
        public static IQueryable<T> GetPageData<T>(this IQueryable<T> entities, PaginationInfo pageInfo,
            string cacheName = null, bool retrivePagesExist = true) {
            if (pageInfo.PageNumber == null || pageInfo.PageNumber <= 0) {
                pageInfo.PageNumber = 1;
            }

            if (pageInfo.ItemsInPage == null || pageInfo.ItemsInPage == -1)
                pageInfo.ItemsInPage = PageItems;

            var take = (int)pageInfo.ItemsInPage;
            var skip = (int)pageInfo.PageNumber * take - take; //5 * 10 - 10
            //var hashCode = entities.GetHashCode();
            var cachePages = pageInfo.PagesExists == null ? -1 : (int)pageInfo.PagesExists;
            var saveCache = false;
            if (!string.IsNullOrEmpty(cacheName)) {
                var cachePagesString = Mvc.Caches.GetString(cacheName);
                if (cachePagesString != null) {
                    cachePages = int.Parse(cachePagesString);
                    pageInfo.PagesExists = cachePages;
                }
                saveCache = true;
            }
            if (cachePages < 0 && retrivePagesExist) {
                decimal pagesExist = 1;
                pagesExist = entities.Count() / (decimal)pageInfo.ItemsInPage;
                pageInfo.PagesExists = (int)Math.Ceiling(pagesExist);
            } else {
                pageInfo.PagesExists = cachePages;
            }
            if (saveCache) {
                Mvc.Caches.Set(cacheName, pageInfo.PagesExists);
            }
            return entities.Skip(skip).Take(take);
        }

        /// <summary>
        ///     Generates list items with unordered list with bootstrap pagination.
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="unorderedListClass"></param>
        /// <param name="withoutUnorderedList"></param>
        /// <param name="liClass"></param>
        /// <param name="cacheName"></param>
        /// <param name="class"></param>
        /// <param name="maxNumbersOfPagesShow"></param>
        /// <param name="format"></param>
        /// <param name="activeClass"></param>
        /// <returns></returns>
        public static string GetList(
            PaginationInfo pageInfo,
            string url = "url/@page",
            string title = "titleContent of ... at @page", string content = "@page", string unorderedListClass = "",
            bool withoutUnorderedList = true, string @liClass = "", string cacheName = "", string @class = "",
            int maxNumbersOfPagesShow = 5,
            string format =
                "<li class='@is-active-state @liClass' data-page='@page'><a href='@url' class='@class' title='@title' >@content</a></li>",
            string activeClass = "active") {
            // code started
            string firstPageLink = "",
                lastPageLink = "",
                appendingListItem = "";

            var sb = new StringBuilder(maxNumbersOfPagesShow + 10);

            var sampleListItem = format.Replace("@url", url);
            sampleListItem = sampleListItem.Replace("@title", title);
            sampleListItem = sampleListItem.Replace("@class", @class);
            sampleListItem = sampleListItem.Replace("@liClass", @liClass);
            var startPageNumber = 1;
            if (withoutUnorderedList == false) {
                var ulStart = "<ul class='" + unorderedListClass + "'>";
                sb.AppendLine(ulStart);
            }
            var endPageNumber = (int)pageInfo.PagesExists;

            if (pageInfo.PageNumber == null) {
                return "";
            }
            if (pageInfo.PagesExists > maxNumbersOfPagesShow) {
                // pages are higher than max number
                firstPageLink = sampleListItem.Replace("@content", "First");
                firstPageLink = firstPageLink.Replace("@page", "1");

                var mid = (int)Math.Ceiling(maxNumbersOfPagesShow / (decimal)2); // 5/2 = 2
                startPageNumber = (int)pageInfo.PageNumber - mid;
                sb.AppendLine(firstPageLink);
                if (startPageNumber <= 0) {
                    startPageNumber = 0 - startPageNumber;
                    endPageNumber += startPageNumber;
                    startPageNumber = 1;
                }
            }
            for (var i = startPageNumber; i <= endPageNumber; i++) {
                appendingListItem = sampleListItem.Replace(@"@content", content);
                appendingListItem = appendingListItem.Replace(@"@page", i.ToString());
                if (i == pageInfo.PageNumber) {
                    //active
                    appendingListItem = appendingListItem.Replace("@is-active-state", "active");
                } else {
                    appendingListItem = appendingListItem.Replace("@is-active-state", "");
                }
                sb.AppendLine(appendingListItem);
            }
            if (pageInfo.PagesExists > maxNumbersOfPagesShow) {
                lastPageLink = sampleListItem.Replace("@content", "Last");
                lastPageLink = lastPageLink.Replace("@page", endPageNumber.ToString());
                sb.AppendLine(lastPageLink);
            }
            if (withoutUnorderedList == false) {
                sb.AppendLine("</ul>");
            }
            var output = sb.ToString();
            sb = null;
            GC.Collect();
            return output;
        }
    }
}