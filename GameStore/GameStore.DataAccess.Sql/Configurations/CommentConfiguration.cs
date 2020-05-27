using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Body).IsRequired();

            builder.HasQueryFilter(c => !c.IsDeleted);

            builder
                .HasOne(c => c.GameRoot)
                .WithMany(g => g.Comments)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithMany()
                .IsRequired();
        }
    }
}