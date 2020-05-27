using GameStore.Core.Models.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DataAccess.Sql.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(notification => notification.Id);

            builder.HasData
            (
                new Notification {Id = "1", Name = "E-mail"},
                new Notification {Id = "2", Name = "SMS"},
                new Notification {Id = "3", Name = "Push notifications"}
            );
        }
    }
}