using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Common.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Models.ViewModels
{
    public class BanViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required] 
        public BanTerm BanTerm { get; set; }
        
        public IEnumerable<SelectListItem> Terms { get; set; }
    }
}