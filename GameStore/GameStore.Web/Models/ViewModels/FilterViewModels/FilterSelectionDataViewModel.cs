using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Models.ViewModels.FilterViewModels
{
    public class FilterSelectionDataViewModel
    {
        public IEnumerable<ListItem> Genres { get; set; }
        public IEnumerable<ListItem> Platforms { get; set; }
        public IEnumerable<ListItem> Publishers { get; set; }
        public IEnumerable<ListItem> PublishingDates { get; set; }
        public IEnumerable<LinkItem> SortOptions { get; set; }
        public IEnumerable<LinkItem> PageSizes { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}