using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels.ImageViewModels
{
    public class ImageIndexViewModel
    {
        public string GameKey { get; set; }
        public IEnumerable<GameImageViewModel> Images { get; set; }
    }
}