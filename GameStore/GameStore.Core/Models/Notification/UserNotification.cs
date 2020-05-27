using GameStore.Core.Models.Identity;

namespace GameStore.Core.Models.Notification
{
    public class UserNotification
    {
        public string NotificationId { get; set; }
        public Notification Notification { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}