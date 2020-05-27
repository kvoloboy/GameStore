using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.RoleViewModels
{
    public class ModifyRoleViewModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "NameMessage")]
        public string Name { get; set; }
        
        public IEnumerable<string> SelectedPermissions { get; set; } = new List<string>();
        public IEnumerable<ListItem> Permissions { get; set; }
    }
}