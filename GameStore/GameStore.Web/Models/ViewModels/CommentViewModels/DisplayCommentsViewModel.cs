using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels.CommentViewModels
{
    public class DisplayCommentsViewModel
    {
        public CommentViewModel Comment { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}