using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Models.ViewModels.ImageViewModels
{
    public class ModifyImageViewModel
    {
        public string Id { get; set; }
        
        [Required]
        public IFormFile Image { get; set; }
        
        [Required]
        public string GameKey { get; set; }
    }
}