using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace DevMVCComponent.Database {
    /// <summary>
    /// Generates pagination
    /// </summary>
    public static class Pagination {

        private static long _pageItems = 50;
        /// <summary>
        /// Total number of items in a page.
        /// </summary>
        public static long PageItems {
            get {

                return _pageItems;
            }
            set { _pageItems = value; }
        }


        /// <summary>
        /// Get pagination data based on the page number with cached pages count.
        /// </summary>
        /// <param name="entities">Sent Entities as IQueryable</param>
        /// <param name="pages">Send a ref of integer to get the pages number. It will be generated from method. Use it to generate pages. It indicated how many pages exist</param>
        /// <param name="page">Based on the page number it returns data. </param>
        /// <param name="items">How many items should a page contain? (Default is 30 defined in the PageItems Property of the class)</param>
        /// <param name="retrivePagesExist">If not false then no pages count will be generated from database.</param>
        /// <returns>IQueryable data based on the page number.</returns>
        public static IQueryable<T> GetPageData<T>(this IQueryable<T> entities, string cacheName, ref int pages, long? page = 1, long? items = -1, bool retrivePagesExist = true) {
            if (page == null && page <= 0) {
                page = 1;
            }

            if (items == null || items == -1)
                items = PageItems;

            var take = (int)items;
            int skip = (int)page * take - take; //5 * 10 - 10
            //var hashCode = entities.GetHashCode();
            var cachePagesString = Starter.Caches.Get(cacheName);
            int cachePages = -1;
            if (cachePagesString != null) {
                cachePages = long.Parse(cachePagesString);
            }
            if (cachePages < 0 && retrivePagesExist) {
                decimal pagesExist = 1;
                pagesExist = entities.Count() / (decimal)items;
                pages = (int)Math.Ceiling(pagesExist);
                Starter.Caches.Set(cacheName, pages);
            }

            return entities.Skip(skip).Take(take);
        }
        /// <summary>
        /// Get pagination data based on the page number with cached pages count.
        /// </summary>
        /// <param name="entities">Sent Entities as IQueryable</param>
        /// <param name="pageInfo">Send a ref of integer to get the pages number. It will be generated from method. Use it to generate pages. It indicated how many pages exist</param>
        /// <returns>IQueryable data based on the page number.</returns>
        public static IQueryable<T> GetPageData<T>(this IQueryable<T> entities, string cacheName, PaginationInfo pageInfo, bool retrivePagesExist = true) {
            if (pageInfo.PageNumber == null && pageInfo.PageNumber <= 0) {
                pageInfo.PageNumber = 1;
            }

            if (pageInfo.ItemsInPage == null || pageInfo.ItemsInPage == -1)
                pageInfo.ItemsInPage = PageItems;

            var take = (int)pageInfo.ItemsInPage;
            int skip = (int)pageInfo.PageNumber * take - take; //5 * 10 - 10
            //var hashCode = entities.GetHashCode();
            var cachePagesString = Starter.Caches.Get(cacheName);
            int cachePages = -1;
            if (cachePagesString != null) {
                cachePages = long.Parse(cachePagesString);
            }
            if (cachePages < 0 && retrivePagesExist) {
                decimal pagesExist = 1;
                pagesExist = entities.Count() / (decimal)pageInfo.ItemsInPage;
                pageInfo.PagesExists = (int)Math.Ceiling(pagesExist);
                Starter.Caches.Set(cacheName, pageInfo.PagesExists);
            } else {
                pageInfo.PagesExists = cachePages;
            }

            return entities.Skip(skip).Take(take);
        }
        /// <summary>
        /// Generates list items with unordered list with bootstrap pagination.
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="unorderedListClass"></param>
        /// <param name="liClass"></param>
        /// <param name="cacheName"></param>
        /// <param name="class"></param>
        /// <param name="maxNumbersOfPagesShow"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static MvcHtmlString GetList(PaginationInfo pageInfo, string url="url/@page", string content = "@page", string title = "titleContent of ... at @page", string unorderedListClass = "", string @liClass = "", string cacheName = "", string @class = "", int maxNumbersOfPagesShow = 5, string format = "<li class='@is-active-state @liClass'><a href='@url' class='@class' title='@title' >@content</a></li>") {
            string sampleListItem = format.Replace("@url", url);
            sampleListItem = sampleListItem.Replace("@title", title);
            sampleListItem = sampleListItem.Replace("@class", @class);
            sampleListItem = sampleListItem.Replace("@liClass", @liClass);
            StringBuilder sb = new StringBuilder(maxNumbersOfPagesShow + 2);
            int startPageNumber = 1;
            string ulStart = "<ul class='" + unorderedListClass + "'>";
            string appendingListItem = "";
            sb.AppendLine(ulStart);
            int endPageNumber = pageInfo.PagesExists;

            if (pageInfo.PageNumber == null) {
                return new MvcHtmlString("");
            }
            string firstPageLink = "", lastPageLink = "";
            if (pageInfo.PagesExists > maxNumbersOfPagesShow) {
                // pages are higher than max number
                firstPageLink = sampleListItem.Replace("@content", "First");
                firstPageLink = firstPageLink.Replace("@page", "1");

                int mid = (int)Math.Ceiling(maxNumbersOfPagesShow / (decimal)2); // 5/2 = 2
                startPageNumber = (int)pageInfo.PageNumber - mid;
                sb.AppendLine(firstPageLink);
                if (startPageNumber <= 0) {
                    startPageNumber = 0 - startPageNumber;
                    endPageNumber += startPageNumber;
                    startPageNumber = 1;
                }

            }
            for (int i = startPageNumber; i <= endPageNumber; i++) {
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
            sb.AppendLine("</ul>");
            return new MvcHtmlString(sb.ToString());
        }

    }
}