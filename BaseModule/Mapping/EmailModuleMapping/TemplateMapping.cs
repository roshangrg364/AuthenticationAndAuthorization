using EmailModule.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModule.Mapping.EmailModuleMapping
{
    public class TemplateMapping : IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property<long>(a => a.Id).HasColumnName("template_id").HasMaxLength(20).IsRequired();
            builder.Property<string>(a => a.Template).HasColumnName("template").IsRequired();
            builder.Property<string>(a => a.Type).HasColumnName("type").IsRequired();
            builder.Ignore(a => a.TemplateVariables);
            builder.ToTable("template");
        }
    }
}
