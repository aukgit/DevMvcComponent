using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevMVCComponent.Database {
    public class PaginationInfo {
        /// <summary>
        /// Set from user side to indicate the user numbers.
        /// </summary>
        public long?  ItemsInPage { get; set; }
        /// <summary>
        /// Receive from method
        /// </summary>
        public int? PagesExists { get; set; }

        /// <summary>
        /// Set from user side. To get the items for that page.
        /// </summary>
        public int? PageNumber { get; set; }

    }
}