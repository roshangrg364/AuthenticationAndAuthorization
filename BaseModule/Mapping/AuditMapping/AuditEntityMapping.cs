using BaseModule.AuditManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseModule.Mapping.AuditMapping
{
    public class AuditEntityMapping : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {

            builder
                .HasKey(a => a.Id);



            builder
                    .Property(a => a.UserId)
                    .HasColumnName("user_id");
            builder
                   .Property(a => a.Type)
                   .HasColumnName("type")
                   .IsRequired();


            builder
                  .Property(a => a.TableName)
                   .HasColumnName("table_name")
                  .HasMaxLength(100)
                  .IsRequired();
            builder
            
                 .Property(a => a.ActionOn)
                  .HasColumnName("action_on")
             .HasDefaultValue(DateTime.Now)
             .IsRequired();


            builder
               .Property(a => a.OldValues)
            .HasColumnName("old_values");

            builder
               .Property(a => a.NewValues)
               .HasColumnName("new_values");
              builder
               .Property(a => a.PrimaryKey)
               .HasColumnName("keys");


            builder
                .ToTable("audit_logs");
        }
    }
}
