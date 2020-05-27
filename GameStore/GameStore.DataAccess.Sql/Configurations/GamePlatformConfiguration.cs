using System.Linq;
using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class GamePlatformConfiguration : IEntityTypeConfiguration<GamePlatform>
    {
        public void Configure(EntityTypeBuilder<GamePlatform> builder)
        {
            builder.HasKey(gp => new {gp.GameRootId, gp.PlatformId});

            builder.HasOne(gp => gp.GameRoot)
                .WithMany(g => g.GamePlatforms);

            builder.HasOne(gp => gp.Platform)
                .WithMany(p => p.GamePlatforms);

            var platforms = new[] {"1", "2", "3", "4"};
            builder.HasData
            (
                Enumerable.Range(1, 100).Select((platform, i) =>
                {
                    var gameId = ((i + 2) / 2).ToString();
                    var platformId = platforms[(i + 1) % 3];
                    var gamePlatform = new GamePlatform
                    {
                        GameRootId = gameId,
                        PlatformId = platformId
                    };

                    return gamePlatform;
                })
            );
        }
    }
}