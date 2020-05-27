using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.CommentViewModels
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        
        [Required]
        public string GameKey { get; set; }
        
        [Required(ErrorMessage = "NameMessage")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "BodyMessage")]
        public string Body { get; set; }
        
        public string HtmlId { get; set; }
        public string QuoteText { get; set; }
        public string ParentId { get; set; }
        public CommentViewModel Parent { get; set; }
        public IEnumerable<CommentViewModel> Children { get; set; } = new List<CommentViewModel>();
    }
}