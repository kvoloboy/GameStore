using System;
using System.Collections.Generic;

namespace GameStore.BusinessLayer.DTO
{
    public class ModifyGameDto
    {
        public string Id { get; set; }
        public string DetailsId { get; set; }
        public string Key { get; set; }
        public string PublisherEntityId { get; set; }
        public short? UnitsInStock { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int UnitsOnOrder { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<string> SelectedGenres { get; set; }
        public IEnumerable<string> SelectedPlatforms { get; set; }
        public ICollection<GameLocalizationDto> Localizations { get; set; } = new List<GameLocalizationDto>();
    }
}