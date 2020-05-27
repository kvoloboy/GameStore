using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class UserCultureConfiguration : IEntityTypeConfiguration<UserCulture>
    {
        public void Configure(EntityTypeBuilder<UserCulture> builder)
        {
            builder.HasKey(culture => culture.Name);

            builder.HasData
            (
                new UserCulture {Name = "en-US"},
                new UserCulture {Name = "ru-RU"}
            );
        }
    }
}