using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IShipperService
    {
        Task<IEnumerable<Shipper>> GetAllAsync();
    }
}