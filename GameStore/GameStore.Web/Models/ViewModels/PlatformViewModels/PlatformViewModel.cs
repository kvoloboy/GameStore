using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.PlatformViewModels
{
    public class PlatformViewModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "NameMessage")]
        public string Name { get; set; }
    }
}