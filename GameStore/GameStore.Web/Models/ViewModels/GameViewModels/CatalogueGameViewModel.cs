using System.Collections.Generic;
using GameStore.Web.Models.ViewModels.PageViewModels;

namespace GameStore.Web.Models.ViewModels.GameViewModels
{
    public class GamesCatalogueViewModel
    {
        public IEnumerable<DisplayGameViewModel> Games { get; set; }
        public PageOptionsViewModel PageOptions { get; set; }
    }
}