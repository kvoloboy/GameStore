using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels.GenreViewModels
{
    public class GenreViewModel
    {
        public ModifyGenreViewModel ModifyGenreViewModel { get; set; }
        public IEnumerable<ModifyGenreViewModel> Parents { get; set; }
    }
}