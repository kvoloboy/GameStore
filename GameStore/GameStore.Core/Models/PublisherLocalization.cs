using System;

namespace GameStore.Core.Models
{
    public class PublisherLocalization
    {
        public static bool operator ==(PublisherLocalization left, PublisherLocalization right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PublisherLocalization left, PublisherLocalization right)
        {
            return !Equals(left, right);
        }

        public string Id { get; set; }
        public string PublisherEntityId { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }

        public string CultureName { get; set; }
        public UserCulture UserCulture { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is PublisherLocalization localization))
            {
                return false;
            }

            var areEquals =
                ContactName == localization.ContactName &&
                ContactTitle == localization.ContactTitle &&
                Address == localization.Address &&
                City == localization.City &&
                Region == localization.Region &&
                Country == localization.Country &&
                Description == localization.Description &&
                CultureName == localization.CultureName;

            return areEquals;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(PublisherEntityId);
            hashCode.Add(ContactName);
            hashCode.Add(ContactTitle);
            hashCode.Add(Address);
            hashCode.Add(City);
            hashCode.Add(Region);
            hashCode.Add(Country);
            hashCode.Add(Description);
            hashCode.Add(CultureName);
            
            return hashCode.ToHashCode();
        }
    }
}