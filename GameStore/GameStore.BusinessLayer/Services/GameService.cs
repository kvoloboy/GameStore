using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.BusinessLayer.Sort.Factories.Interfaces;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class GameService : IGameService
    {
        private const int MaxDiscountValue = 100;
        private const string KeySeparator = "_";

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameDecorator _gameDecorator;
        private readonly ISortOptionFactory<GameRoot> _sortOptionFactory;
        private readonly IPublisherService _publisherService;

        public GameService(
            IUnitOfWork unitOfWork,
            IGameDecorator gameDecorator,
            ISortOptionFactory<GameRoot> sortOptionFactory,
            IMapper mapper,
            IPublisherService publisherService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _publisherService = publisherService;
            _gameDecorator = gameDecorator;
            _sortOptionFactory = sortOptionFactory;
        }

        public async Task<string> GenerateKeyAsync(string gameName, string separator)
        {
            if (string.IsNullOrWhiteSpace(gameName))
            {
                throw new InvalidServiceOperationException("Is empty game name");
            }

            var key = gameName.ToLower().Replace(" ", separator);
            var exists = await _gameDecorator.AnyAsync(g => g.Key == key);

            if (!exists)
            {
                return key;
            }

            var uniqueKey = await GenerateUniqueKeyAsync(key);

            return uniqueKey;
        }

        public async Task<IEnumerable<GameDto>> GetAllAsync(string culture = Culture.En)
        {
            var games = await _gameDecorator.FindAllAsync(new GameFilterData());
            var gamesDto = new List<GameDto>();
            
            foreach (var game in games)
            {
                var gameDto = await GetGameDtoAsync(game, culture);
                gamesDto.Add(gameDto);
            }

            return gamesDto;
        }

        public async Task<PageList<IEnumerable<GameDto>>> GetPageListAsync(
            GameFilterData filterData,
            string culture = "en-US")
        {
            if (filterData == null)
            {
                throw new InvalidServiceOperationException("Is null filter data");
            }

            var filteredGames = await _gameDecorator.FindAllAsync(filterData);
            var sortedGames = Sort(filteredGames, filterData.SortOption);
            var gamesDto = new List<GameDto>();
            
            foreach (var game in sortedGames)
            {
                var gameDto = await GetGameDtoAsync(game, culture);
                gamesDto.Add(gameDto);
            }
            
            var pageList = CreatePageList(gamesDto, filterData.PageSize, filterData.PageNumber);

            return pageList;
        }

        public async Task<GameDto> GetByIdAsync(string id, string culture)
        {
            var gameRoot = await _gameDecorator.FindSingleAsync(g => g.Id == id);

            if (gameRoot == null)
            {
                throw new EntityNotFoundException<GameRoot>(id);
            }

            var gameDto = await GetGameDtoAsync(gameRoot, culture, true);

            return gameDto;
        }

        public async Task<ModifyGameDto> GetByIdAsync(string id)
        {
            var gameRoot = await _gameDecorator.FindSingleAsync(g => g.Id == id);
            var modifyGameDto = _mapper.Map<ModifyGameDto>(gameRoot);

            return modifyGameDto;
        }

        public async Task<GameDto> GetByKeyAsync(string key, string culture = Culture.En)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidServiceOperationException("Is empty key");
            }

            var gameRoot = await _gameDecorator.FindSingleAsync(g => g.Key == key);

            if (gameRoot == null)
            {
                throw new EntityNotFoundException<GameRoot>();
            }

            var gameDto = await GetGameDtoAsync(gameRoot, culture, true);

            return gameDto;
        }

        public async Task<IEnumerable<GameDto>> GetByGenreAsync(string genreId)
        {
            var games = await _gameDecorator.FindAllAsync(g => g.GameGenres.Any(genre => genre.GenreId == genreId));
            var gamesDto = new List<GameDto>();
            
            foreach (var game in games)
            {
                var gameDto = await GetGameDtoAsync(game, Culture.En);
                gamesDto.Add(gameDto);
            }

            return gamesDto;
        }

        public async Task<IEnumerable<GameDto>> GetByPublisherAsync(string publisherId)
        {
            var gameFilterData = new GameFilterData
            {
                Publishers = new[] {publisherId}
            };

            var games = await _gameDecorator.FindAllAsync(gameFilterData);
            var gamesDto = new List<GameDto>();
            
            foreach (var game in games)
            {
                var gameDto = await GetGameDtoAsync(game, Culture.En);
                gamesDto.Add(gameDto);
            }

            return gamesDto;
        }

        public async Task CreateAsync(ModifyGameDto gameDto)
        {
            if (gameDto == null)
            {
                throw new InvalidServiceOperationException("Is null game dto");
            }

            if (string.IsNullOrWhiteSpace(gameDto.Key))
            {
                var name = gameDto.Localizations.First(localization => localization.CultureName == Culture.En).Name;
                gameDto.Key = await GenerateKeyAsync(name, KeySeparator);
            }
            else
            {
                await ValidateGameKeyExistingAsync(gameDto.Key);
            }

            var game = _mapper.Map<GameRoot>(gameDto);
            game.Details.CreationDate = DateTime.UtcNow;
            await _gameDecorator.AddAsync(game);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(ModifyGameDto gameDto)
        {
            if (gameDto == null)
            {
                throw new InvalidServiceOperationException("Is null game dto");
            }

            var exist = await _gameDecorator.AnyAsync(g => g.Id == gameDto.Id);

            if (!exist)
            {
                throw new EntityNotFoundException<GameRoot>(gameDto.Id);
            }

            if (string.IsNullOrWhiteSpace(gameDto.Key))
            {
                var gameName = gameDto.Localizations
                    .FirstOrDefault(localization => localization.CultureName == Culture.En)?.Name;

                gameDto.Key = await GenerateKeyAsync(gameName, KeySeparator);
            }
            else
            {
                await ValidateGameKey(gameDto.Id, gameDto.Key);
            }

            var gameRoot = _mapper.Map<GameRoot>(gameDto);
            await _gameDecorator.UpdateAsync(gameRoot);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var exist = await _gameDecorator.AnyAsync(g => g.Id == id);

            if (!exist)
            {
                throw new EntityNotFoundException<GameRoot>(id);
            }

            await _gameDecorator.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task IncrementVisitsCountAsync(string gameKey)
        {
            var gameRoot = await _gameDecorator.FindSingleAsync(g => g.Key == gameKey);

            if (gameRoot == null)
            {
                throw new EntityNotFoundException<GameRoot>();
            }

            var visitRepository = _unitOfWork.GetRepository<IAsyncRepository<Visit>>();

            var visit = await visitRepository.FindSingleAsync(v => v.GameRoot.Key == gameKey);

            if (visit == null)
            {
                visit = new Visit
                {
                    Value = 1,
                    GameRootId = gameRoot.Id
                };
                await visitRepository.AddAsync(visit);
            }
            else
            {
                visit.Value++;
                await visitRepository.UpdateAsync(visit);
            }

            await _unitOfWork.CommitAsync();
        }

        public decimal ComputePriceWithDiscount(decimal price, decimal discount)
        {
            var priceWithDiscount = price - price * discount / MaxDiscountValue;

            return priceWithDiscount;
        }

        public AppFile GetFile(string gameKey)
        {
            const string mimeType = "application/json";
            var data = Encoding.UTF8.GetBytes(gameKey);

            return new AppFile
            {
                Data = data,
                Mime = mimeType
            };
        }

        public Task<int> CountAsync()
        {
            var count = _gameDecorator.CountAsync();

            return count;
        }

        public async Task<GameDto> GetGameDtoAsync(GameRoot gameRoot, string culture, bool includePublisher = false)
        {
            var gameLocalization =
                gameRoot.Localizations.FirstOrDefault(localization => localization.CultureName == culture) ??
                gameRoot.Localizations.FirstOrDefault(localization => localization.CultureName == Culture.En);

            var gameDto = _mapper.Map<GameDto>(gameRoot);
            _mapper.Map(gameLocalization, gameDto);

            if (includePublisher && !string.IsNullOrEmpty(gameDto.PublisherEntityId))
            {
                gameDto.Publisher =
                    await _publisherService.GetByIdAsync(gameDto.PublisherEntityId, gameLocalization?.CultureName);
            }

            return gameDto;
        }

        private async Task ValidateGameKey(string id, string key)
        {
            var existingGameDto = await GetByIdAsync(id);
            var isChangedKey = existingGameDto.Key != key;

            if (isChangedKey)
            {
                await ValidateGameKeyExistingAsync(key);
            }
        }

        private async Task ValidateGameKeyExistingAsync(string key)
        {
            var existsByKey = await _gameDecorator.AnyAsync(g => g.Key == key);

            if (existsByKey)
            {
                throw new EntityExistsWithKeyValueException<GameRoot>(nameof(GameRoot.Key), key);
            }
        }

        private async Task<string> GenerateUniqueKeyAsync(string key)
        {
            var similarKeys = (await _gameDecorator.FindAllAsync(g => g.Key.StartsWith(key)))
                .Select(g => g.Key)
                .ToArray();

            var counter = 1;
            var keyPrefix = key;
            var currentName = key;

            while (ExistsKey(similarKeys, currentName))
            {
                currentName = $"{keyPrefix}{++counter}";
            }

            return currentName;
        }

        private static bool ExistsKey(IEnumerable<string> keys, string targetKey)
        {
            return keys.Any(key => key == targetKey);
        }

        private IEnumerable<GameRoot> Sort(IEnumerable<GameRoot> games, string sortCriteria)
        {
            var sortOption = _sortOptionFactory.Create(sortCriteria);
            var propertyAccessor = sortOption.SortPropertyAccessor.Compile();
            var sortedGames = sortOption.SortDirection == SortDirection.Ascending
                ? games.OrderBy(propertyAccessor)
                : games.OrderByDescending(propertyAccessor);

            return sortedGames.ToList();
        }

        private static PageList<IEnumerable<GameDto>> CreatePageList(
            IEnumerable<GameDto> games,
            string pageSize,
            int pageNumber)
        {
            var pageListModel = GetPageListModel(games, pageSize, pageNumber);
            var pageOptions = CreatePageOptions(pageSize, pageNumber, games.Count());
            var pageList = new PageList<IEnumerable<GameDto>>
            {
                Model = pageListModel,
                PageOptions = pageOptions
            };

            if (!pageList.Model.Any())
            {
                return pageList;
            }

            pageList.MinPrice = (int) Math.Floor(games.Min(g => g.Price));
            pageList.MaxPrice = (int) Math.Ceiling(games.Max(g => g.Price));

            return pageList;
        }

        private static IEnumerable<GameDto> GetPageListModel(
            IEnumerable<GameDto> games,
            string pageSize,
            int pageNumber)
        {
            if (!int.TryParse(pageSize, out var pageSizeNumber))
            {
                return games;
            }

            var skipGamesValue = pageSizeNumber * (pageNumber - 1);
            var pageModel = games.Skip(skipGamesValue).Take(pageSizeNumber);

            return pageModel;
        }

        private static PageOptions CreatePageOptions(string pageSize, int pageNumber, int totalItems)
        {
            var pageOptions = new PageOptions
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalItems = totalItems
            };

            return pageOptions;
        }
    }
}