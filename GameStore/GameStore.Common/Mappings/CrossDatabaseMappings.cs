using AutoMapper;
using GameStore.Common.Mappings.Converters;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Models;
using MongoPublisher = GameStore.DataAccess.Mongo.Models.Publisher;
using Publisher = GameStore.Core.Models.Publisher;

namespace GameStore.Common.Mappings
{
    public class CrossDatabaseMappings : Profile
    {
        public CrossDatabaseMappings()
        {
            CreateMap<MongoPublisher, PublisherLocalization>(MemberList.None)
                .ForMember(localization => localization.PublisherEntityId,
                    options => options.MapFrom(publisher => publisher.Id))
                .ForMember(localization => localization.Id, options => options.Ignore())
                .ForMember(localization => localization.CultureName, options =>
                    options.MapFrom(publisher => Culture.En));

            CreateMap<PublisherLocalization, MongoPublisher>(MemberList.None)
                .ForMember(publisher => publisher.Id, options => options.Ignore());

            CreateMap<MongoPublisher, Publisher>(MemberList.None)
                .ConvertUsing<MongoPublisherToDomainPublisherConverter>();

            CreateMap<Publisher, MongoPublisher>(MemberList.None);

            CreateMap<Product, GameLocalization>(MemberList.None)
                .ForMember(localization => localization.Id, options => options.Ignore())
                .ForMember(localization => localization.CultureName,
                    options => options.MapFrom(product => Culture.En))
                .ForMember(localization => localization.Name, options =>
                    options.MapFrom(product => product.ProductName));

            CreateMap<Product, GameDetails>(MemberList.None).ConvertUsing<ProductToGameDetailsConverter>();

            CreateMap<GameRoot, Product>(MemberList.None);

            CreateMap<GameDetails, Product>(MemberList.None)
                .ForMember(product => product.Id, options => options.Ignore())
                .ForMember(product => product.Discontinued, options =>
                    options.MapFrom(details => details.IsDiscontinued))
                .ForMember(product => product.UnitPrice, options =>
                    options.MapFrom(details => details.Price));

            CreateMap<GameLocalization, Product>(MemberList.None)
                .ForMember(product => product.Id, options => options.Ignore())
                .ForMember(product => product.ProductName, options =>
                    options.MapFrom(product => product.Name));

            CreateMap<Product, GameRoot>(MemberList.None).ConvertUsing<ProductToGameRootConverter>();
        }
    }
}