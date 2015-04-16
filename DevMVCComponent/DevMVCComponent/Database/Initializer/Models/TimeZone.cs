using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevMVCComponent.Database.Initializer.Models {
    public partial class TimeZone {
        public TimeZone() 
        {
            //default
        }

        public short TimeZoneID { get; set; }
        public string TimeZoneInfoId { get; set; }
        public string TimeZoneDisplay { get; set; }

    }
}