using System.Collections.Generic;
using GameStore.Web.Models.ViewModels.FilterViewModels;

namespace GameStore.Web.Models.ViewModels.GameViewModels
{
    public class GameIndexViewModel
    {
        public IEnumerable<DisplayGameViewModel> DisplayGameViewModels { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}