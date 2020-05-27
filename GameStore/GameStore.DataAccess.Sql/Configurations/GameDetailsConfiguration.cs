using System;
using System.Linq;
using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class GameDetailsConfiguration : IEntityTypeConfiguration<GameDetails>
    {
        public void Configure(EntityTypeBuilder<GameDetails> builder)
        {
            builder.HasKey(g => g.Id);
            
            builder.Property(g => g.Id).ValueGeneratedOnAdd();
            builder.Property(g => g.Discount).HasColumnType("decimal");
            builder.Property(g => g.Price).HasColumnType("money");
            builder.Property(g => g.UnitsInStock).HasColumnType("smallint");
            
            builder.HasOne(g => g.GameRoot)
                .WithOne(gr => gr.Details)
                .IsRequired();

            builder.HasData
            (
                Enumerable.Range(1, 50).Select((g, i) =>
                {
                    var id = (i + 1).ToString();

                    return new GameDetails
                    {
                        Id = id,
                        GameRootId = id,
                        Price = 10000 / (i + 1),
                        CreationDate = new DateTime(2019, 9, 9),
                        Discount = 5
                    };
                })
            );
        }
    }
}