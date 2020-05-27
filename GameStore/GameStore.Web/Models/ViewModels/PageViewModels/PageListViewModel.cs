using GameStore.Web.Models.ViewModels.FilterViewModels;
using GameStore.Web.Models.ViewModels.GameViewModels;

namespace GameStore.Web.Models.ViewModels.PageViewModels
{
    public class PageListViewModel
    {
        public GamesCatalogueViewModel Catalogue { get; set; }
        public FilterViewModel Filter { get; set; }
        public bool IsDeleted => Filter.FilterSelectedOptionsViewModel.IsDeleted;
    }
}