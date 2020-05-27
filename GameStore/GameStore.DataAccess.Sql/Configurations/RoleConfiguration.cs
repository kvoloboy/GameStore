using GameStore.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(g => g.Id).ValueGeneratedOnAdd();
            builder.Property(r => r.Name).IsRequired();

            builder.HasIndex(r => r.Name).IsUnique();

            builder.HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role);

            builder.HasData
            (
                new Role {Id = "1", Name = "Admin"},
                new Role {Id = "2", Name = "Manager"},
                new Role {Id = "3", Name = "Moderator"},
                new Role {Id = "4", Name = "User"},
                new Role {Id = "5", Name = "Publisher"},
                new Role {Id = "6", Name = "Guest"}
            );
        }
    }
}