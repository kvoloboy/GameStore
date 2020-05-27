using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Models.ViewModels.UserViewModels
{
    public class AssignToPublisherViewModel
    {
        [Required]
        public string UserId { get; set; }
        public string PublisherId { get; set; }
        public IEnumerable<SelectListItem> Publishers { get; set; }
    }
}