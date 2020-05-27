using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels.PageViewModels
{
    public class PageOptionsViewModel
    {
        public string PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int EndPage { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public ICollection<LinkItem> Pages { get; set; } = new List<LinkItem>();
        public LinkItem PreviousPage { get; set; }
        public LinkItem NextPage { get; set; }
    }
}