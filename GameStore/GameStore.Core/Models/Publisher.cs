using System;
using System.Collections.Generic;
using GameStore.Core.Models.Identity;

namespace GameStore.Core.Models
{
    public class Publisher
    {
        public Publisher()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string HomePage { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<GameRoot> GameRoots { get; set; }
        public bool CanBeUsed { get; set; }
        public ICollection<PublisherLocalization> Localizations { get; set; }

        public static readonly Publisher None = new Publisher
        {
            CompanyName = "Undefined",
            HomePage = string.Empty
        };

        public override bool Equals(object obj)
        {
            if (!(obj is Publisher publisher))
            {
                return false;
            }

            var areEquals =
                Id == publisher.Id &&
                CompanyName == publisher.CompanyName &&
                PostalCode == publisher.PostalCode &&
                Phone == publisher.Phone &&
                Fax == publisher.Fax &&
                HomePage == publisher.HomePage;

            return areEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Id,
                CompanyName,
                PostalCode,
                Phone,
                Fax,
                HomePage);
        }
    }
}