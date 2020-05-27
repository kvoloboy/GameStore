using System.Linq;
using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class GameLocalizationConfiguration : IEntityTypeConfiguration<GameLocalization>
    {
        public void Configure(EntityTypeBuilder<GameLocalization> builder)
        {
            builder.HasKey(localization => localization.Id);

            builder.Property(g => g.Name).IsRequired();
            builder.Property(g => g.Id).ValueGeneratedOnAdd();

            builder.HasOne(localization => localization.GameRoot)
                .WithMany(root => root.Localizations)
                .IsRequired();

            builder.HasOne(localization => localization.UserCulture)
                .WithMany()
                .HasForeignKey(localization => localization.CultureName)
                .IsRequired();
            
            builder.HasData
            (
                Enumerable.Range(1, 100).Select((localization, i) =>
                {
                    var id = (i + 1).ToString();
                    var gameId = ((i + 2) / 2).ToString();

                    return new GameLocalization
                    {
                        Id = id,
                        GameRootId = gameId,
                        CultureName = (i + 1) % 2 == 0 ? "ru-RU" : "en-US",
                        Name = (i + 1) % 2 == 0 ? "Имя" : "Name"
                    };
                })
            );
        }
    }
}