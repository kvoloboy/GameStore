namespace GameStore.BusinessLayer.DTO
{
    public class PublisherDto
    {
        public string Id { get; set; }
        public string LocalizationId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Description { get; set; }
        public string HomePage { get; set; }
        public string CultureName { get; set; }
        public string UserId { get; set; }
        public bool CanBeUsed { get; set; }
    }
}