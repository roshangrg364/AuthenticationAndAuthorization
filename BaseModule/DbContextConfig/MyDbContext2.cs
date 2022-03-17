
using BaseModule.ActivityManagement.Entity;
using BaseModule.AuditManagement;
using BaseModule.Mapping.InventoryMapping;
using BaseModule.Mapping.User;
using InventoryModule.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserModule.Entity;
namespace BaseModule.DbContextConfig
{
   public partial class MyDbContext: IdentityDbContext
    {
        #region Inventory
        public DbSet<Category> Category { get; set; }
        #endregion

        #region Audit and activity log
        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        #endregion
    }
}
