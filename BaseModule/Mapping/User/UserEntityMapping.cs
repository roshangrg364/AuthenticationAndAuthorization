using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseModule.Mapping.User
{
    public class UserEntityMapping : IEntityTypeConfiguration<UserModule.Entity.User>
    {
        public void Configure(EntityTypeBuilder<UserModule.Entity.User> builder)
        {

            builder
                .ToTable("AspNetUsers")
                  .Property(a => a.Name)
                  .HasMaxLength(100)
                  .IsRequired();

            builder
              .ToTable("AspNetUsers")
                .Property(a => a.CreatedOn)
                .IsRequired();


            builder
                .ToTable("AspNetUsers")
                  .Property(a => a.Status)
                  .HasMaxLength(100)
                  .IsRequired();
        }
    }
}
