using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.GenreViewModels
{
    public class ModifyGenreViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "NameMessage")]
        public string Name { get; set; }
        
        public int? ParentId { get; set; }
    }
}