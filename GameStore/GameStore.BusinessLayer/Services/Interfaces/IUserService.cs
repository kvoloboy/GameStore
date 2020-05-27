using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.Common.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> CreateGuestAsync();
        Task CreateAsync(UserDto userDto, string password);
        Task MergeUserActionsAsync(string guestId, string userId);
        Task UpdateRolesAsync(string userId, IEnumerable<string> roles);
        Task<string> TrySignInAsync(string email, string password);
        Task AssignToPublisherAsync(string userId, string publisherId);
        Task BanAsync(string userId, BanTerm term);
        Task<UserDto> GetByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        UserDto GetDefaultUserDto(string id, string email);
    }
}