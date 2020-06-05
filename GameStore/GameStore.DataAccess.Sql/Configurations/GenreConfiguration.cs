using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(g => g.Id);
            
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(g => g.Name).IsRequired();

            builder.HasQueryFilter(g => !g.IsDeleted);
            
            builder
                .HasMany(genre => genre.GameGenres)
                .WithOne(gameGenre => gameGenre.Genre);
                
            builder.HasData
            (
                new Genre{Id = "1", Name = "Strategy"},
                new Genre{Id = "2", Name = "RTS", ParentId = "1"},
                new Genre{Id = "3", Name = "TBS", ParentId = "1"},
                new Genre{Id = "4", Name = "RPG"},
                new Genre{Id = "5", Name = "Sports"},
                new Genre{Id = "6", Name = "Races"},
                new Genre{Id = "7", Name = "Rally", ParentId = "6"},
                new Genre{Id = "8", Name = "Arcade", ParentId = "6"},
                new Genre{Id = "9", Name = "Formula", ParentId = "6"},
                new Genre{Id = "10", Name = "Off-road", ParentId = "6"},
                new Genre{Id = "11", Name = "Action"},
                new Genre{Id = "12", Name = "FPS", ParentId = "11"},
                new Genre{Id = "13", Name = "TPS", ParentId = "11"},
                new Genre{Id = "14", Name = "Adventure"},
                new Genre{Id = "15", Name = "Puzzle & Skill"}
            );
        }
    }
}
