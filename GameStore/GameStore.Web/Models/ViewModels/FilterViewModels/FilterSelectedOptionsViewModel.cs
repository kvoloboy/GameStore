using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PageSizeList = GameStore.Common.Models.PageSize;

namespace GameStore.Web.Models.ViewModels.FilterViewModels
{
    public class FilterSelectedOptionsViewModel
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string CreationDate { get; set; }
        public string Name { get; set; }
        public string SortOption { get; set; } = Common.Models.SortOptions.New;
        public int PageNumber { get; set; } = 1;
        public string PageSize { get; set; } = PageSizeList.Ten;
        public IEnumerable<string> Genres { get; set; }
        public IEnumerable<string> Platforms { get; set; }
        public IEnumerable<string> Publishers { get; set; }
        public bool IsDeleted { get; set; }
    }
}