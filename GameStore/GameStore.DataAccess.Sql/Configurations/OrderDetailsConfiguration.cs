using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.HasKey(od => od.Id);
            
            builder.Property(od => od.Price).HasColumnType("money");
            builder.Property(od => od.Quantity).HasColumnType("smallint");
            builder.Property(od => od.Discount).HasColumnType("real");
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            
            builder
                .HasOne(od => od.GameRoot)
                .WithMany()
                .IsRequired();

            builder.HasOne(od => od.Order)
                .WithMany(o => o.Details)
                .IsRequired();
        }
    }
}