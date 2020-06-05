using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CompanyName).IsRequired();

            builder.Property(p => p.CompanyName).HasColumnType("nvarchar(40)");
            builder.Property(p => p.HomePage).HasColumnType("ntext");

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Ignore(p => p.GameRoots);
            builder.Ignore(p => p.CanBeUsed);
            builder.Ignore(p => p.Localizations);

            builder.HasQueryFilter(p => !p.IsDeleted);

            builder.HasData
            (
                new Publisher {Id = "101", CompanyName = "Воздушный Андрей", HomePage = "https://www.microsoft.com/"},
                new Publisher {Id = "102", CompanyName = "Пахучий Антон", HomePage = "https://www.google.com/"},
                new Publisher {Id = "103", CompanyName = "Помытый Филип", HomePage = "https://www.amazon.com/"}
            );
        }
    }
}
