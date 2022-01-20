
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
    public class MyDbContext:IdentityDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        #region Inventory
        public DbSet<Category> Category { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region inventorymapping
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            #endregion

            #region userMapping
            modelBuilder.ApplyConfiguration(new UserEntityMapping());
            #endregion

        }
    }
}
