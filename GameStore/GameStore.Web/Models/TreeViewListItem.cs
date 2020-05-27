using System.Collections.Generic;

namespace GameStore.Web.Models
{
    public class TreeViewListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string ParentId { get; set; }
        public IEnumerable<TreeViewListItem> Children { get; set; }
    }
}