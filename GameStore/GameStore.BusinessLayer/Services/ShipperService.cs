using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IAsyncReadonlyRepository<Shipper> _shipperRepository;

        public ShipperService(IAsyncReadonlyRepository<Shipper> shipperRepository)
        {
            _shipperRepository = shipperRepository;
        }

        public async Task<IEnumerable<Shipper>> GetAllAsync()
        {
            var shippers = await _shipperRepository.FindAllAsync();

            return shippers;
        }
    }
}