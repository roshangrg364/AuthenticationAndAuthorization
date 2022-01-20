using InventoryModule.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseModule.Mapping.InventoryMapping
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .ToTable("category")
                   .HasKey(a => a.Id);

            builder
                  .ToTable("category")
                   .Property(a => a.Id)
                   .HasMaxLength(11)
                   .HasColumnName("category_id")
                   .IsRequired();

            builder
                  .ToTable("category")
                   .Property(a => a.Name)
                   .HasMaxLength(100)
                   .HasColumnName("name")
                   .IsRequired();

            builder
                  .ToTable("category")
                   .Property(a => a.Status)
                   .HasMaxLength(100)
                   .HasColumnName("status")
                   .IsRequired();
        }

    }
}
