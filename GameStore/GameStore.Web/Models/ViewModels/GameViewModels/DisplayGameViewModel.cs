using System;
using System.Collections.Generic;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using GameStore.Web.Models.ViewModels.PublisherViewModels;

namespace GameStore.Web.Models.ViewModels.GameViewModels
{
    public class DisplayGameViewModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string UnitsInStock { get; set; }
        public decimal UnitsOnOrder { get; set; }
        public string QuantityPerUnit { get; set; }
        public IEnumerable<string> Genres { get; set; } = new List<string>();
        public IEnumerable<string> Platforms { get; set; } = new List<string>();
        public PublisherViewModel Publisher { get; set; }
        public DateTime CreationDate { get; set; }
        public DisplayCommentsViewModel DisplayCommentsViewModel { get; set; }
        public int CommentsCount { get; set; }
        public RatingViewModel Rating { get; set; }
        public IList<GameImageViewModel> Images { get; set; }
    }
}