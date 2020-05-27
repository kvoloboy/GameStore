using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Mappings.Converters
{
    public class ModifyGameDtoToGameRootConverter : ITypeConverter<ModifyGameDto, GameRoot>
    {
        public GameRoot Convert(ModifyGameDto source, GameRoot destination, ResolutionContext context)
        {
            var root = CreateRoot(source, context.Mapper);

            return root;
        }

        private static GameRoot CreateRoot(ModifyGameDto modifyGameDto, IMapper mapper)
        {
            var rootId = modifyGameDto.Id ?? Guid.NewGuid().ToString();
            
            var root = new GameRoot
            {
                Id = rootId,
                Key = modifyGameDto.Key,
                PublisherEntityId = modifyGameDto.PublisherEntityId,
                GameGenres = GetGameGenres(modifyGameDto),
                GamePlatforms = GetGamePlatforms(modifyGameDto),
                Localizations = mapper.Map<IEnumerable<GameLocalization>>(modifyGameDto.Localizations).ToList(),
                Details = GetDetails(modifyGameDto, rootId, mapper)
            };

            return root;
        }
        
        private static ICollection<GameGenre> GetGameGenres(ModifyGameDto modifyGameDto)
        {
            var gameGenres = modifyGameDto.SelectedGenres.Select(g => new GameGenre
            {
                GenreId = g,
                GameRootId = modifyGameDto.Id
            }).ToList();

            return gameGenres;
        }

        private static ICollection<GamePlatform> GetGamePlatforms(ModifyGameDto modifyGameDto)
        {
            var gamePlatforms = modifyGameDto.SelectedPlatforms.Select(p => new GamePlatform
            {
                PlatformId = p,
                GameRootId = modifyGameDto.Id
            }).ToList();

            return gamePlatforms;
        }
        
        private static GameDetails GetDetails(ModifyGameDto dto, string rootId, IMapper mapper)
        {
            var details = mapper.Map<GameDetails>(dto);
            details.GameRootId = rootId;

            return details;
        }
    }
}