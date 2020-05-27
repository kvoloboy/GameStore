using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Factories
{
    public class ShipmentViewModelFactory : IAsyncViewModelFactory<ShipmentViewModel, ShipmentViewModel>
    {
        private readonly IShipperService _shipperService;
        private readonly IMapper _mapper;

        public ShipmentViewModelFactory(IShipperService shipperService, IMapper mapper)
        {
            _shipperService = shipperService;
            _mapper = mapper;
        }

        public async Task<ShipmentViewModel> CreateAsync(ShipmentViewModel model)
        {
            var viewModel = _mapper.Map<ShipmentViewModel>(model);
            var shippers = await _shipperService.GetAllAsync();
            var shipperListItems = shippers.Select(s => new SelectListItem
            {
                Value = s.Id,
                Text = s.CompanyName,
                Selected = s.Id == model.ShipperEntityId
            });
            viewModel.Shippers = shipperListItems;

            return viewModel;
        }
    }
}