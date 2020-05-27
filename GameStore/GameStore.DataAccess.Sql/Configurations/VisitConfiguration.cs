using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            
            builder.HasOne(v => v.GameRoot)
                .WithOne(gr => gr.Visit)
                .IsRequired();
        }
    }
}