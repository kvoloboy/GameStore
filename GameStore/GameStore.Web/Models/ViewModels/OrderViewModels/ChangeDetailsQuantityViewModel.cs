using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class ChangeDetailsQuantityViewModel
    {
        [Required]
        public string Id { get; set; }
        [Range(0, short.MaxValue, ErrorMessage = "PositiveValue")] 
        public short Quantity { get; set; }
        public string GameName { get; set; }
        public string GameUnitsInStock { get; set; }
    }
}