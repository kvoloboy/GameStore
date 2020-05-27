using System;
using System.Collections.Generic;
using GameStore.Core.Models.Notification;

namespace GameStore.Core.Models.Identity
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? BannedTo { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Rating> GameRatings { get; set; }
        public ICollection<UserNotification> Notifications { get; set; }
    }
}