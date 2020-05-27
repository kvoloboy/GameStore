using System.Linq;
using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
    {
        public void Configure(EntityTypeBuilder<GameGenre> builder)
        {
            builder.HasKey(gg => new {GameId = gg.GameRootId, gg.GenreId});

            builder.HasOne(gg => gg.GameRoot)
                .WithMany(g => g.GameGenres);

            builder.HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres);
                
            var genres = new[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"};
            builder.HasData(Enumerable.Range(1, 100).Select((genre, i) =>
            {
                var gameId = ((i + 2) / 2).ToString();
                var genreId = genres[(i + 1) % 14];
                return new GameGenre
                {
                    GameRootId = gameId,
                    GenreId = genreId
                };
            }));
        }
    }
}