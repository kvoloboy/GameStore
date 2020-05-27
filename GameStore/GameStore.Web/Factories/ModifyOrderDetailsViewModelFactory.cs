using System.Linq;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Factories
{
    public class ModifyOrderDetailsViewModelFactory : IAsyncViewModelFactory<ModifyOrderDetailsViewModel, ModifyOrderDetailsViewModel>
    {
        private readonly IGameService _gameService;

        public ModifyOrderDetailsViewModelFactory(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<ModifyOrderDetailsViewModel> CreateAsync(ModifyOrderDetailsViewModel model)
        {
            var games = (await _gameService.GetAllAsync(Culture.Current)).ToList();
            var listItems = games.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id,
                Selected = g.Id == model.GameId
            });
            model.Games = listItems;

            return model;
        }
    }
}