using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            
            builder.Ignore(o => o.Shipper);
            
            builder.HasMany(o => o.Details)
                .WithOne(od => od.Order);

            builder.HasOne(o => o.User)
                .WithMany()
                .IsRequired();
        }
    }
}