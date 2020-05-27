using GameStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.HasKey(pt => pt.Id);
            
            builder.HasData
            (
                new PaymentType
                {
                    Id = "1",
                    Title = "Bank",
                    Description = "Best bank in the world",
                    ImagePath = "/img/Bank.png"
                },
                new PaymentType
                {
                    Id = "2",
                    Title = "IBox terminal",
                    Description = "Best IBox terminal in the world",
                    ImagePath = "/img/IBox.png"
                },
                new PaymentType
                {
                    Id = "3",
                    Title = "Visa", 
                    Description = "Best Visa in the world",
                    ImagePath = "/img/Visa.png"
                }
            );
        }
    }
}