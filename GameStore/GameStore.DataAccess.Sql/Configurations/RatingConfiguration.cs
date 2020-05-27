using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(rating => rating.Id);
            builder.Property(rating => rating.Id).ValueGeneratedOnAdd();

            builder.HasOne(rating => rating.User)
                .WithMany(user => user.GameRatings)
                .IsRequired();

            builder.HasOne(rating => rating.GameRoot)
                .WithMany(root => root.GameRatings)
                .IsRequired();
        }
    }
}