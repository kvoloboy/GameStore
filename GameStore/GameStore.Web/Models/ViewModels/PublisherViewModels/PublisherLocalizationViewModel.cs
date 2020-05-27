namespace GameStore.Web.Models.ViewModels.PublisherViewModels
{
    public class PublisherLocalizationViewModel
    {
        public string Id { get; set; }
        public string PublisherId { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string CultureName { get; set; }

        public bool IsAssigned()
        {
            var isAssigned =
                ContactName != default ||
                ContactTitle != default ||
                Address != default ||
                City != default ||
                Region != default ||
                Country != default ||
                Description != default;

            return isAssigned;
        }
    }
}