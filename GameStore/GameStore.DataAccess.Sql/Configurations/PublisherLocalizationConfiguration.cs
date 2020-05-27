using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class PublisherLocalizationConfiguration : IEntityTypeConfiguration<PublisherLocalization>
    {
        public void Configure(EntityTypeBuilder<PublisherLocalization> builder)
        {
            builder.HasKey(localization => localization.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Description).HasColumnType("ntext");

            builder.HasOne(p => p.UserCulture)
                .WithMany()
                .HasForeignKey(p => p.CultureName)
                .IsRequired();
                
            builder.HasData
            (
                new PublisherLocalization
                {
                    Id = "1",
                    PublisherEntityId = "1",
                    Description = "Best company in the world",
                    CultureName = "en-US"
                },
                new PublisherLocalization
                {
                    Id = "2",
                    PublisherEntityId = "1",
                    Description = "Самая лучшая компания в мире",
                    CultureName = "ru-RU"
                },
                new PublisherLocalization
                {
                    Id = "3",
                    PublisherEntityId = "2",
                    Description = "Good company",
                    CultureName = "en-US"
                },
                new PublisherLocalization
                {
                    Id = "4",
                    PublisherEntityId = "2",
                    Description = "Хорошая компания",
                    CultureName = "ru-RU"
                },
                new PublisherLocalization
                {
                    Id = "5",
                    PublisherEntityId = "3",
                    Description = "Not bad",
                    CultureName = "en-US"
                }
                ,new PublisherLocalization
                {
                    Id = "6",
                    PublisherEntityId = "3",
                    Description = "Ничё так",
                    CultureName = "ru-RU"
                }
            );
        }
    }
}