using System.Collections.Generic;
using GameStore.Web.Models.ViewModels.PublisherViewModels;

namespace GameStore.Web.Models.ViewModels.GameViewModels
{
    public class GameSelectionDataViewModel
    {
        public IEnumerable<TreeViewListItem> Genres { get; set; }
        public IEnumerable<ListItem> Platforms { get; set; }
        public IEnumerable<PublisherViewModel> Publishers { get; set; }
    }
}