using System.Collections.Generic;

namespace GameStore.Common.Models
{
    public class GameFilterData
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string CreationDate { get; set; }
        public string Name { get; set; }
        public string SortOption { get; set; }
        public int PageNumber { get; set; }
        public string PageSize { get; set; }
        public IEnumerable<string> Keys { get; set; } = new List<string>();
        public IEnumerable<string> Genres { get; set; }
        public IEnumerable<string> Platforms { get; set; }
        public IEnumerable<string> Publishers { get; set; }
        public bool AreDeleted { get; set; } = false;
    }
}