using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DB = DevMVCComponent.Database.Initializer.Models;

namespace DevMVCComponent.Database.Initializer {
    public class DevOrgDbContext : DbContext {
        public DbSet<DB.TimeZone> TimeZones { get; set; }

    }
}