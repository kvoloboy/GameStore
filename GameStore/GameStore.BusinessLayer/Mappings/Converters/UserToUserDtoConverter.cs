using System.Linq;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Mappings.Converters
{
    public class UserToUserDtoConverter : ITypeConverter<User, UserDto>
    {
        public UserDto Convert(User source, UserDto destination, ResolutionContext context)
        {
            var dto = new UserDto
            {
                Id = source.Id,
                Email = source.Email,
                BannedTo = source.BannedTo,
                Roles = source.UserRoles?.Select(ur => new RoleDto
                {
                    Id = ur.RoleId,
                    Name = ur.Role.Name,
                    Permissions = ur.Role?.RolePermissions.Select(rp => rp.Permission.Value)
                }).ToList(),
                SelectedNotifications = source.Notifications?.Select(notification => notification.NotificationId)
            };

            return dto;
        }
    }
}