using System;
using System.Collections.Generic;

namespace GameStore.BusinessLayer.DTO
{
    public class GameDto
    {
        public string Id { get; set; }
        public string DetailsId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string PublisherEntityId { get; set; }
        public string Description { get; set; }
        public short? UnitsInStock { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int UnitsOnOrder { get; set; }
        public string QuantityPerUnit { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public RatingDto RatingDto { get; set; }
        public PublisherDto Publisher { get; set; }
        public IEnumerable<string> SelectedGenres { get; set; }
        public IEnumerable<string> SelectedPlatforms { get; set; }
        public ICollection<GameImageDto> Images { get; set; }
    }
}