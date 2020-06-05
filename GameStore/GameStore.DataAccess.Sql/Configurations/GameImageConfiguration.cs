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
            var path1 = Path.Combine("wwwroot", "img", "01-hotel-palace-merano-health-for-life.jpg");
            var path2 = Path.Combine("wwwroot", "img", "binori-hotel.jpg");
            var path3 = Path.Combine("wwwroot", "img", "dji-0032.jpg");

            var image1 = File.ReadAllBytes(path1);
            var image2 = File.ReadAllBytes(path2);
            var image3 = File.ReadAllBytes(path3);

            var gameImage1 = new GameImage
            {
                Id = "1",
                Content = image1,
                GameRootId = "1",
                ContentType = "image/png"
            };

            var gameImage2 = new GameImage
            {
                Id = "2",
                Content = image2,
                GameRootId = "1",
                ContentType = "image/png"
            };
            var gameImage3 = new GameImage
            {
                Id = "3",
                Content = image3,
                GameRootId = "1",
                ContentType = "image/png"
            };


            builder.HasData(gameImage1, gameImage2, gameImage3);
        }
    }
}
