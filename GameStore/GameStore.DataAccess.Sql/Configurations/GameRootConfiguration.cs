using System.Linq;
using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class GameRootConfiguration : IEntityTypeConfiguration<GameRoot>
    {
        public void Configure(EntityTypeBuilder<GameRoot> builder)
        {
            builder.HasKey(gr => gr.Id);

            builder.HasIndex(g => g.Key).IsUnique();

            builder.Property(g => g.Key).IsRequired();
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Ignore(g => g.Publisher);

            builder.HasOne(gr => gr.Details)
                .WithOne(gd => gd.GameRoot);

            builder.HasOne(g => g.Visit)
                .WithOne(v => v.GameRoot);

            builder.HasMany(g => g.Comments)
                .WithOne(c => c.GameRoot);

            builder.HasMany(g => g.GameGenres)
                .WithOne(c => c.GameRoot);

            builder.HasMany(g => g.GamePlatforms)
                .WithOne(c => c.GameRoot);
            
            InitData(builder);
        }

        private static void InitData(EntityTypeBuilder<GameRoot> builder)
        {
            var publishers = new[] {"101", "102", "103"};

            builder.HasData(
                Enumerable.Range(1, 50).Select((gr, i) =>
                {
                    var id = (i + 1).ToString();
                    var key = $"key{id}";

                    return new GameRoot
                    {
                        Id = id,
                        Key = key,
                        Visit = null,
                        PublisherEntityId = publishers[(i+1) % 3]
                    };
                })
            );
        }
    }
}