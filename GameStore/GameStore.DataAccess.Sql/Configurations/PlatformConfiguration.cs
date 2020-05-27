using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired();
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.HasQueryFilter(p => !p.IsDeleted);

            builder
                .HasMany(p => p.GamePlatforms)
                .WithOne(gpj => gpj.Platform);

            builder.HasData
            (
                new Platform {Id = "1", Name = "Console"},
                new Platform {Id = "2", Name = "Desktop"},
                new Platform {Id = "3", Name = "Mobile"},
                new Platform {Id = "4", Name = "Browser"}
            );
        }
    }
}