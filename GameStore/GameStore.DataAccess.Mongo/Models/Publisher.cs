using System;

namespace GameStore.DataAccess.Mongo.Models
{
    public class Publisher
    {
        public string Id { get; set; }
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

        public override bool Equals(object other)
        {
            if (!(other is Publisher publisher))
            {
                return false;
            }

            return Id == publisher.Id &&
                   CompanyName == publisher.CompanyName &&
                   ContactName == publisher.ContactName &&
                   ContactTitle == publisher.ContactTitle &&
                   Address == publisher.Address &&
                   City == publisher.City &&
                   Region == publisher.Region &&
                   PostalCode == publisher.PostalCode &&
                   Country == publisher.Country &&
                   Phone == publisher.Phone &&
                   Fax == publisher.Fax &&
                   Description == publisher.Description &&
                   HomePage == publisher.HomePage;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(CompanyName);
            hashCode.Add(ContactName);
            hashCode.Add(ContactTitle);
            hashCode.Add(Address);
            hashCode.Add(City);
            hashCode.Add(Region);
            hashCode.Add(PostalCode);
            hashCode.Add(Country);
            hashCode.Add(Phone);
            hashCode.Add(Fax);
            hashCode.Add(Description);
            hashCode.Add(HomePage);

            return hashCode.ToHashCode();
        }
    }
}