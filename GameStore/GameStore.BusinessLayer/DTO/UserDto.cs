using System;
using System.Collections.Generic;

namespace GameStore.BusinessLayer.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public DateTime? BannedTo { get; set; }
        public ICollection<RoleDto> Roles { get; set; }
        public IEnumerable<string> SelectedNotifications { get; set; }
    }
}