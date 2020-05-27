using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.UserViewModels
{
    public class ModifyUserViewModel
    {
        [Required]
        public string Id { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
        public IEnumerable<ListItem> Roles { get; set; }
    }
}