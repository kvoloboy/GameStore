using System.Security.Claims;
using System.Threading.Tasks;

namespace GameStore.Identity.Factories.Interfaces
{
    public interface IClaimsPrincipalFactory
    {
        Task<ClaimsPrincipal> CreateAsync(string userId);
    }
}