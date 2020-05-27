using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.GameViewModels
{
    public class ModifyGameViewModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string DetailsId { get; set; }
        public string LocalizationId { get; set; }

        [Required(ErrorMessage = "NameMessage")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "PositiveValue")] 
        public short? UnitsInStock { get; set; }
        
        [Range(0.0, double.MaxValue, ErrorMessage = "PositiveValue")]
        public decimal Price { get; set; }
        
        [Range(0.0, 100.0, ErrorMessage = "RangeMessage")]
        public decimal Discount { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a positive value")]
        public int UnitsOnOrder { get; set; }
        public string QuantityPerUnit { get; set; }
        public IEnumerable<string> SelectedGenres { get; set; }
        public IEnumerable<string> SelectedPlatforms { get; set; }
        public string PublisherEntityId { get; set; }
        public GameLocalizationViewModel GameLocalization { get; set; }
    }
}