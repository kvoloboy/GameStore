using System.IO;
using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class GameImageConfiguration : IEntityTypeConfiguration<GameImage>
    {
        public void Configure(EntityTypeBuilder<GameImage> builder)
        {
            builder.HasKey(image => image.Id);

            builder.Property(image => image.Id).ValueGeneratedOnAdd();

            builder.HasOne(image => image.GameRoot)
                .WithMany(root => root.GameImages)
                .IsRequired();

            InitData(builder);
        }

        private static void InitData(EntityTypeBuilder<GameImage> builder)
        {
            var path = Path.Combine("wwwroot", "img", "visa.png");
            var image = File.ReadAllBytes(path);

            var gameImage = new GameImage
            {
                Id = "1",
                Content = image,
                GameRootId = "1",
                ContentType = "image/png"
            };

            builder.HasData(gameImage);
        }
    }
}