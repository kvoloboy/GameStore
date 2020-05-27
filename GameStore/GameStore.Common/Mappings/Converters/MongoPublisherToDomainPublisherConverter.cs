using System.Collections.Generic;
using AutoMapper;
using GameStore.Common.Models;
using GameStore.Core.Models;
using MongoPublisher = GameStore.DataAccess.Mongo.Models.Publisher;

namespace GameStore.Common.Mappings.Converters
{
    public class MongoPublisherToDomainPublisherConverter : ITypeConverter<MongoPublisher, Publisher>
    {
        public Publisher Convert(MongoPublisher source, Publisher destination, ResolutionContext context)
        {
            var publisherDetailsLocalization = context.Mapper.Map<PublisherLocalization>(source);
            publisherDetailsLocalization.CultureName = Culture.En;
            var publisher = new Publisher
            {
                Id = source.Id,
                CompanyName = source.CompanyName,
                Fax = source.Fax,
                Phone = source.Phone,
                HomePage = source.HomePage,
                PostalCode = source.PostalCode,
                Localizations = new List<PublisherLocalization>
                {
                    publisherDetailsLocalization
                }
            };

            return publisher;
        }
    }
}