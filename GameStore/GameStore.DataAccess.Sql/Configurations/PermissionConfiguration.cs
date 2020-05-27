using GameStore.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(g => g.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Value).IsRequired();

            builder.HasIndex(p => p.Value).IsUnique();

            builder.HasData
            (
                new Permission {Id = "1", Value = "Read deleted games"},
                new Permission {Id = "2", Value = "Create game"},
                new Permission {Id = "3", Value = "Update game"},
                new Permission {Id = "4", Value = "Delete game"},
                new Permission {Id = "5", Value = "Create genre"},
                new Permission {Id = "6", Value = "Update genre"},
                new Permission {Id = "7", Value = "Delete genre"},
                new Permission {Id = "8", Value = "Create platform"},
                new Permission {Id = "9", Value = "Update platform"},
                new Permission {Id = "10", Value = "Delete platform"},
                new Permission {Id = "11", Value = "Create publisher"},
                new Permission {Id = "12", Value = "Update publisher"},
                new Permission {Id = "13", Value = "Delete publisher"},
                new Permission {Id = "14", Value = "Create comment"},
                new Permission {Id = "15", Value = "Delete comment"},
                new Permission {Id = "16", Value = "Ban user"},
                new Permission {Id = "17", Value = "Read orders"},
                new Permission {Id = "18", Value = "Update order"},
                new Permission {Id = "19", Value = "Make order"},
                new Permission {Id = "20", Value = "Create role"},
                new Permission {Id = "21", Value = "Update role"},
                new Permission {Id = "22", Value = "Delete role"},
                new Permission {Id = "23", Value = "Setup roles"},
                new Permission {Id = "24", Value = "Read users"},
                new Permission {Id = "25", Value = "Read roles"},
                new Permission {Id = "26", Value = "Read personal orders"},
                new Permission {Id = "27", Value = "Manage images"},
                new Permission {Id = "28", Value = "Rate game"},
                new Permission {Id = "29", Value = "Subscribe on notifications"}
            );
        }
    }
}