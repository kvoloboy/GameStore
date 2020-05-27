using GameStore.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => new {rp.RoleId, rp.PermissionId});

            builder.HasOne(rp => rp.Permission)
                .WithMany()
                .IsRequired();

            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .IsRequired();

            builder.HasData
            (
                new RolePermission{RoleId = "1", PermissionId = "1"},
                new RolePermission{RoleId = "1", PermissionId = "2"},
                new RolePermission{RoleId = "1", PermissionId = "3"},
                new RolePermission{RoleId = "1", PermissionId = "4"},
                new RolePermission{RoleId = "1", PermissionId = "5"},
                new RolePermission{RoleId = "1", PermissionId = "6"},
                new RolePermission{RoleId = "1", PermissionId = "7"},
                new RolePermission{RoleId = "1", PermissionId = "8"},
                new RolePermission{RoleId = "1", PermissionId = "9"},
                new RolePermission{RoleId = "1", PermissionId = "10"},
                new RolePermission{RoleId = "1", PermissionId = "11"},
                new RolePermission{RoleId = "1", PermissionId = "12"},
                new RolePermission{RoleId = "1", PermissionId = "13"},
                new RolePermission{RoleId = "1", PermissionId = "14"},
                new RolePermission{RoleId = "1", PermissionId = "15"},
                new RolePermission{RoleId = "1", PermissionId = "16"},
                new RolePermission{RoleId = "1", PermissionId = "17"},
                new RolePermission{RoleId = "1", PermissionId = "18"},
                new RolePermission{RoleId = "1", PermissionId = "19"},
                new RolePermission{RoleId = "1", PermissionId = "20"},
                new RolePermission{RoleId = "1", PermissionId = "21"},
                new RolePermission{RoleId = "1", PermissionId = "22"},
                new RolePermission{RoleId = "1", PermissionId = "23"},
                new RolePermission{RoleId = "1", PermissionId = "24"},
                new RolePermission{RoleId = "1", PermissionId = "25"},
                new RolePermission{RoleId = "1", PermissionId = "26"},
                new RolePermission{RoleId = "1", PermissionId = "27"},
                new RolePermission{RoleId = "1", PermissionId = "28"},

                new RolePermission{RoleId = "2", PermissionId = "1"},
                new RolePermission{RoleId = "2", PermissionId = "2"},
                new RolePermission{RoleId = "2", PermissionId = "3"},
                new RolePermission{RoleId = "2", PermissionId = "4"},
                new RolePermission{RoleId = "2", PermissionId = "5"},
                new RolePermission{RoleId = "2", PermissionId = "6"},
                new RolePermission{RoleId = "2", PermissionId = "7"},
                new RolePermission{RoleId = "2", PermissionId = "8"},
                new RolePermission{RoleId = "2", PermissionId = "9"},
                new RolePermission{RoleId = "2", PermissionId = "10"},
                new RolePermission{RoleId = "2", PermissionId = "11"},
                new RolePermission{RoleId = "2", PermissionId = "12"},
                new RolePermission{RoleId = "2", PermissionId = "13"},
                new RolePermission{RoleId = "2", PermissionId = "19"},
                new RolePermission{RoleId = "2", PermissionId = "26"},
                new RolePermission{RoleId = "2", PermissionId = "27"},
                new RolePermission{RoleId = "2", PermissionId = "28"},
                new RolePermission{RoleId = "2", PermissionId = "29"},
                
                new RolePermission{RoleId = "3", PermissionId = "1"},
                new RolePermission{RoleId = "3", PermissionId = "14"},
                new RolePermission{RoleId = "3", PermissionId = "15"},
                new RolePermission{RoleId = "3", PermissionId = "16"},
                new RolePermission{RoleId = "3", PermissionId = "19"},
                new RolePermission{RoleId = "3", PermissionId = "26"},
                new RolePermission{RoleId = "3", PermissionId = "28"},

                new RolePermission{RoleId = "4", PermissionId = "14"},
                new RolePermission{RoleId = "4", PermissionId = "19"},
                new RolePermission{RoleId = "4", PermissionId = "26"},
                new RolePermission{RoleId = "4", PermissionId = "28"},

                new RolePermission{RoleId = "5", PermissionId = "3"},
                new RolePermission{RoleId = "5", PermissionId = "12"},
                new RolePermission{RoleId = "5", PermissionId = "14"},
                new RolePermission{RoleId = "5", PermissionId = "19"},
                new RolePermission{RoleId = "5", PermissionId = "26"},
                new RolePermission{RoleId = "5", PermissionId = "28"},

                new RolePermission{RoleId = "6", PermissionId = "14"}
            );
        }
    }
}