
using BaseModule.AuditManagement;
using BaseModule.Mapping.AuditMapping;
using BaseModule.Mapping.InventoryMapping;
using BaseModule.Mapping.User;
using InventoryModule.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserModule.Entity;

namespace BaseModule.DbContextConfig
{
    public partial class MyDbContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public MyDbContext(DbContextOptions<MyDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region inventorymapping
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            #endregion

            #region userMapping
            modelBuilder.ApplyConfiguration(new UserEntityMapping());
            #endregion

            #region audit
            modelBuilder.ApplyConfiguration(new AuditEntityMapping());
            #endregion

        }

        public virtual async Task<int> SaveChangesAsync()
        {
            OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync();
            return result;
        }
        private void OnBeforeSaveChanges()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
              var browser = _httpContextAccessor.HttpContext.Request.Headers["user-agent"].ToString();
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntry.IpAddress = ipAddress;
                auditEntry.Browser = browser;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }



    }
}
