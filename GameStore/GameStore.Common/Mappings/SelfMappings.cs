using AutoMapper;
using GameStore.Common.Mappings.Converters;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.DataAccess.Mongo.Models;

namespace GameStore.Common.Mappings
{
    public class SelfMappings : Profile
    {
        public SelfMappings()
        {
            CreateMap<Comment, Comment>();
            
            CreateMap<Genre, Genre>();
            
            CreateMap<Order, Order>()
                .ForMember(dest => dest.Details, opts => opts.Ignore());
            
            CreateMap<GameRoot, GameRoot>()
                .ForMember(dest => dest.Comments, opts => opts.Ignore())
                .ForMember(dest => dest.Details, opts => opts.Ignore());
            
            CreateMap<GameDetails, GameDetails>()
                .ForPath(dest => dest.GameRoot, opts => opts.Ignore());

            CreateMap<User, User>();

            CreateMap<Role, Role>();
        }
    }
}