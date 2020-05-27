using System.Collections.Generic;

namespace GameStore.BusinessLayer.DTO
{
    public class ModifyPublisherDto
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string HomePage { get; set; }
        public string UserId { get; set; }
        public ICollection<PublisherLocalizationDto> Localizations { get; set; } = new List<PublisherLocalizationDto>();
    }
}