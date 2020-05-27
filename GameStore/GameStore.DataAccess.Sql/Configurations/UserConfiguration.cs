using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GameStore.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.HasQueryFilter(u => !u.IsDeleted);
            builder.HasQueryFilter(u => u.UserRoles.All(r => r.Role.Name != DefaultRoles.Guest));

            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User);
            
            InitData(builder);
        }

        private static void InitData(EntityTypeBuilder<User> builder)
        {
            builder.HasData
            (
                CreateUser("1", "admin@example.com", "12345"),
                CreateUser("2", "manager@example.com", "12345"),
                CreateUser("3", "moderator@example.com", "12345"),
                CreateUser("4", "user@example.com", "12345")
            );
        }

        private static User CreateUser(string id, string email, string password)
        {
            var user = new User
            {
                Id = id,
                Email = email,
                PasswordHash = GetPasswordHash(password)
            };

            return user;
        }

        private static string GetPasswordHash(string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hash = MD5.Create().ComputeHash(passwordBytes);
            var stringRepresentation = new Guid(hash).ToString();

            return stringRepresentation;
        }
    }
}