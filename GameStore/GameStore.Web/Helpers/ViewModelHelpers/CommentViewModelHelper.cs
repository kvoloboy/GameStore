using GameStore.Web.Models.ViewModels.CommentViewModels;

namespace GameStore.Web.Helpers.ViewModelHelpers
{
    public static class CommentViewModelHelper
    {
        public static void SetupHtmlId(CommentViewModel node)
        {
            var id = $"{node.Name}_{node.Id}";
            node.HtmlId = id;

            foreach (var child in node.Children)
            {
                SetupHtmlId(child);
            }

            if (node.Parent != null && node.Parent.HtmlId == null)
            {
                SetupHtmlId(node.Parent);
            }
        }
    }
}