using GameStore.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new {ur.RoleId, ur.UserId});

            builder.HasOne(ur => ur.Role)
                .WithMany()
                .IsRequired();

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .IsRequired();
            
            InitData(builder);
        }

        private static void InitData(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasData
            (
                new UserRole{RoleId = "1", UserId = "1"},
                new UserRole{RoleId = "2", UserId = "2"},
                new UserRole{RoleId = "3", UserId = "3"},
                new UserRole{RoleId = "4", UserId = "4"}
            );
        }
    }
}