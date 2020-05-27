using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.ViewModels.PublisherViewModels
{
    public class ModifyPublisherViewModel
    {
        public string Id { get; set; }
        public string LocalizationId { get; set; }
        
        [Required(ErrorMessage = "CompanyName")]
        [MaxLength(40, ErrorMessage = "MaxLength")]
        public string CompanyName { get; set; }

        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string HomePage { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public PublisherLocalizationViewModel PublisherLocalization { get; set; }
    }
}