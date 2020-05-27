using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.BusinessLayer.Sort.Factories.Interfaces;
using GameStore.BusinessLayer.Sort.Options.Interfaces;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class GameServiceTests
    {
        private const string Id = "1";
        private const string TestPageSize = "3";
        private const string Key = "key";
        private const string Separator = "_";
        private const string Name = "Name";

        private IUnitOfWork _unitOfWork;
        private IGameDecorator _gameDecorator;
        private IMapper _mapper;
        private IAsyncRepository<Visit> _visitRepository;
        private ISortOptionFactory<GameRoot> _sortOptionFactory;
        private ISortOption<GameRoot> _sortOption;
        private IPublisherService _publisherService;
        private GameService _gameServices;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _gameDecorator = A.Fake<IGameDecorator>();
            _mapper = A.Fake<IMapper>();
            _visitRepository = A.Fake<IAsyncRepository<Visit>>();
            _sortOptionFactory = A.Fake<ISortOptionFactory<GameRoot>>();
            _sortOption = A.Fake<ISortOption<GameRoot>>();
            _publisherService = A.Fake<IPublisherService>();

            A.CallTo(() => _sortOption.SortDirection).Returns(SortDirection.Descending);
            A.CallTo(() => _sortOption.SortPropertyAccessor).Returns(root => root.Details.Price);
            A.CallTo(() => _sortOptionFactory.Create(A<string>._)).Returns(_sortOption);
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<Visit>>()).Returns(_visitRepository);

            _gameServices =
                new GameService(_unitOfWork, _gameDecorator, _sortOptionFactory, _mapper, _publisherService);
        }

        [Test]
        public async Task GenerateKeyAsync_ThrowsException_WhenEmptyGameName()
        {
            var name = string.Empty;

            Func<Task> action = async () => await _gameServices.GenerateKeyAsync(name, Separator);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is empty game name");
        }

        [Test]
        public void GenerateKeyAsync_ReturnsKey_WhenValidInput()
        {
            const string expectedKey = "grand_the_auto_san_andreas";
            const string input = "Grand The Auto San Andreas";

            var key = _gameServices.GenerateKeyAsync(input, Separator).Result;

            key.Should().BeEquivalentTo(expectedKey);
        }

        [Test]
        public void GenerateKeyAsync_AddsNumberToKey_WhenExistsSameKey()
        {
            const string expectedKey = "grand4";
            const string input = "Grand";
            var testGamesSequence = CreateGameRoots();
            A.CallTo(() => _gameDecorator.AnyAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns(true);
            A.CallTo(() => _gameDecorator.FindAllAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns(testGamesSequence);

            var key = _gameServices.GenerateKeyAsync(input, Separator).Result;

            key.Should().BeEquivalentTo(expectedKey);
        }

        [Test]
        public void GetAllAsync_CallsDecorator_Always()
        {
            _gameServices.GetAllAsync();

            A.CallTo(() => _gameDecorator.FindAllAsync(A<GameFilterData>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetPageListAsync_ThrowsException_WhenNullArgument()
        {
            Func<Task> action = async () => await _gameServices.GetPageListAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void GetPageListAsync_ReturnsModelWithCountThatEqualsToItemsPerPage_WhenFoundByFilters()
        {
            const int expectedItemsPerPageCount = 3;
            var gameRoots = CreateGameRoots();
            var testGamesDto = CreateGamesDtoSequence();
            var filterData = CreateGameFilterData();
            A.CallTo(() => _gameDecorator.FindAllAsync(filterData)).Returns(gameRoots);
            A.CallTo(() => _mapper.Map<GameDto>(A<GameRoot>._)).ReturnsNextFromSequence(testGamesDto);

            var page = _gameServices.GetPageListAsync(filterData).Result;

            page.Model.Count().Should().Be(expectedItemsPerPageCount);
        }

        [Test]
        public void GetPageListAsync_ReturnsExpectedPageOptions_WhenFoundGames()
        {
            var testGamesDto = CreateGamesDtoSequence();
            var expectedPageOptions = CreatePageOptions();
            var filterData = CreateGameFilterData();
            var gameRoots = CreateGameRoots();
            A.CallTo(() => _gameDecorator.FindAllAsync(filterData)).Returns(gameRoots);
            A.CallTo(() => _mapper.Map<GameDto>(A<GameRoot>._)).ReturnsNextFromSequence(testGamesDto);

            var page = _gameServices.GetPageListAsync(filterData).Result;

            page.PageOptions.Should().BeEquivalentTo(expectedPageOptions);
        }

        [Test]
        public void GetPageListAsync_ReturnsSortedCollection_WhenFoundSortOption()
        {
            var testGamesDto = CreateGamesDtoSequence();
            var expectedGames = GetSortedByPriceAsc();
            var filterData = CreateGameFilterData();
            filterData.PageSize = PageSize.Fifty;
            var gameRoots = CreateGameRoots();
            A.CallTo(() => _gameDecorator.FindAllAsync(filterData)).Returns(gameRoots);
            A.CallTo(() => _mapper.Map<GameDto>(A<GameRoot>._)).ReturnsNextFromSequence(testGamesDto);

            var page = _gameServices.GetPageListAsync(filterData).Result;

            page.Model.Should().BeEquivalentTo(expectedGames);
        }

        [Test]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            A.CallTo(() => _gameDecorator.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns((GameRoot) null);

            Func<Task> action = async () => await _gameServices.GetByIdAsync(Id, Culture.En);

            await action.Should().ThrowAsync<EntityNotFoundException<GameRoot>>()
                .WithMessage($"Entity GameRoot wasn't found. Id: {Id}");
        }

        [Test]
        public void GetByIdAsync_ReturnsDto_WhenFound()
        {
            var dto = _gameServices.GetByIdAsync(Id);

            dto.Should().NotBeNull();
        }

        [Test]
        public async Task GetByKeyAsync_ThrowsException_WhenEmptyKey()
        {
            const string key = "";

            Func<Task> action = async () => await _gameServices.GetByKeyAsync(key);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is empty key");
        }

        [Test]
        public async Task GetByKeyAsync_ThrowsException_WhenNotFound()
        {
            A.CallTo(() => _gameDecorator.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns((GameRoot) null);

            Func<Task> action = async () => await _gameServices.GetByKeyAsync(Key);

            await action.Should().ThrowAsync<EntityNotFoundException<GameRoot>>();
        }

        [Test]
        public void GetByKeyAsync_ReturnsGame_WhenValidKey()
        {
            _gameServices.GetByKeyAsync(Key);

            A.CallTo(() => _gameDecorator.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _gameServices.CreateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is null game dto");
        }

        [Test]
        public void CreateAsync_GeneratesKey_WhenKeyIsEmpty()
        {
            const string expectedKey = "name";
            var gameRoot = CreateGameRoots()[0];
            var dto = CreateModifyGameDto(string.Empty);
            A.CallTo(() => _gameDecorator.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns((GameRoot) null);
            A.CallTo(() => _mapper.Map<GameRoot>(A<ModifyGameDto>._)).Returns(gameRoot);

            _gameServices.CreateAsync(dto);

            dto.Key.Should().BeEquivalentTo(expectedKey);
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenKeyExists()
        {
            const string existingKey = "g";
            var dto = CreateModifyGameDto(existingKey);
            A.CallTo(() => _gameDecorator.AnyAsync(A<Expression<Func<GameRoot, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _gameServices.CreateAsync(dto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<GameRoot>>()
                .WithMessage($"Entity GameRoot with Key : {existingKey} already exists.");
        }

        [Test]
        public void CreateAsync_SavesToRepository_WhenValidDto()
        {
            var dto = CreateModifyGameDto();
            var gameRoot = CreateGameRoots()[0];
            A.CallTo(() => _gameDecorator.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns((GameRoot) null);
            A.CallTo(() => _mapper.Map<GameRoot>(A<ModifyGameDto>._)).Returns(gameRoot);

            _gameServices.CreateAsync(dto);

            A.CallTo(() => _gameDecorator.AddAsync(A<GameRoot>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNullArgument()
        {
            Func<Task> action = async () => await _gameServices.UpdateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is null game dto");
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNotExists()
        {
            var dto = CreateModifyGameDto();
            A.CallTo(() => _gameDecorator.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns((GameRoot) null);

            Func<Task> action = async () => await _gameServices.UpdateAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException<GameRoot>>()
                .WithMessage("Entity GameRoot wasn't found. Id: 1");
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenKeyExists()
        {
            const string existingKey = "g";
            var dto = CreateModifyGameDto(existingKey);
            A.CallTo(() => _gameDecorator.AnyAsync(A<Expression<Func<GameRoot, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _gameServices.UpdateAsync(dto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<GameRoot>>()
                .WithMessage($"Entity GameRoot with Key : {existingKey} already exists.");
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenChangedKeyAndKeyExists()
        {
            var testGameDto = CreateModifyGameDto("key 1");
            A.CallTo(() => _gameDecorator.AnyAsync(A<Expression<Func<GameRoot, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _gameServices.UpdateAsync(testGameDto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<GameRoot>>();
        }

        [Test]
        public void UpdateAsync_CallsRepository_WhenValid()
        {
            var dto = CreateModifyGameDto();
            A.CallTo(() => _gameDecorator.AnyAsync(A<Expression<Func<GameRoot, bool>>>._)).Returns(true);

            _gameServices.UpdateAsync(dto);

            A.CallTo(() => _gameDecorator.UpdateAsync(A<GameRoot>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void IncrementVisitsCountAsync_CreatesNewVisitsWithGameRootId_WhenNotFound()
        {
            A.CallTo(() => _visitRepository.FindSingleAsync(A<Expression<Func<Visit, bool>>>._))
                .Returns((Visit) null);

            _gameServices.IncrementVisitsCountAsync(Key);

            A.CallTo(() => _visitRepository.AddAsync(A<Visit>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void IncrementVisitsCountAsync_IncrementsExistingVisits_WhenFound()
        {
            var visit = new Visit {Value = 1};
            A.CallTo(() => _visitRepository.FindSingleAsync(A<Expression<Func<Visit, bool>>>._)).Returns(visit);

            _gameServices.IncrementVisitsCountAsync(Key);
            visit.Value++;

            A.CallTo(() => _visitRepository.UpdateAsync(visit)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task IncrementVisitsCountAsync_ThrowsException_WhenNotFoundGame()
        {
            A.CallTo(() => _gameDecorator.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns((GameRoot) null);

            Func<Task> action = async () => await _gameServices.IncrementVisitsCountAsync(Key);

            await action.Should().ThrowAsync<EntityNotFoundException<GameRoot>>();
        }

        [Test]
        public void IncrementVisitsCountAsync_CommitsChanges_WhenFoundGame()
        {
            var key = string.Empty;

            _gameServices.IncrementVisitsCountAsync(key);

            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DeleteAsync_ThrowsException_WhenNotExists()
        {
            A.CallTo(() => _gameDecorator.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns((GameRoot) null);

            Func<Task> action = async () => await _gameServices.DeleteAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<GameRoot>>()
                .WithMessage($"Entity GameRoot wasn't found. Id: {Id}");
        }

        [Test]
        public void DeleteAsync_CallsRepository_WhenExistsWithId()
        {
            A.CallTo(() => _gameDecorator.AnyAsync(A<Expression<Func<GameRoot, bool>>>._)).Returns(true);

            _gameServices.DeleteAsync(Id);

            A.CallTo(() => _gameDecorator.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void ComputePriceWithDiscount_ReturnsTotal_WhenNotNullGame()
        {
            const decimal expectedTotal = 90;
            const decimal price = 100;
            const decimal discount = 10;

            var actualTotal = _gameServices.ComputePriceWithDiscount(price, discount);

            actualTotal.Should().Be(expectedTotal);
        }

        [Test]
        public void GetFile_ReturnsNonEmptyFile_Always()
        {
            const string key = "gameKey";

            var file = _gameServices.GetFile(key);

            file.Data.Should().NotBeNull();
        }

        private static ModifyGameDto CreateModifyGameDto(string key = null)
        {
            var dto = new ModifyGameDto
            {
                Id = Id,
                Key = key,
                Localizations = new[]
                {
                    new GameLocalizationDto
                    {
                        CultureName = Culture.En,
                        Name = Name
                    }
                }
            };

            return dto;
        }

        private static GameDto[] CreateGamesDtoSequence()
        {
            var games = new[]
            {
                new GameDto {Id = "1", Key = "grand1", Price = 120},
                new GameDto {Id = "2", Key = "grand2", Price = 120},
                new GameDto {Id = "2", Key = "grand3", Price = 120},
                new GameDto {Id = "2", Key = "grand3", Price = 120},
                new GameDto {Id = "2", Key = "grand3", Price = 100},
                new GameDto {Id = "2", Key = "grand3", Price = 10},
                new GameDto {Id = "2", Key = "grand3", Price = 1100},
                new GameDto {Id = "2", Key = "grand3", Price = 150},
                new GameDto {Id = "2", Key = "grand3", Price = 40},
                new GameDto {Id = "2", Key = "grand3", Price = 110},
                new GameDto {Id = "2", Key = "grand3", Price = 1550}
            };

            return games;
        }

        private static List<GameRoot> CreateGameRoots()
        {
            var roots = new[]
            {
                new GameRoot
                {
                    Id = "1",
                    Key = "grand",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "2",
                    Key = "grand2",
                    Details = new GameDetails()
                },
                new GameRoot
                {
                    Id = "3",
                    Key = "grand3",
                    Details = new GameDetails()
                }
            };

            return roots.ToList();
        }

        private static GameDto[] GetSortedByPriceAsc()
        {
            var sortedGames = new[]
            {
                new GameDto {Id = "2", Key = "grand3", Price = 10},
                new GameDto {Id = "2", Key = "grand3", Price = 40},
                new GameDto {Id = "2", Key = "grand3", Price = 100},
                new GameDto {Id = "2", Key = "grand3", Price = 110},
                new GameDto {Id = "1", Key = "grand1", Price = 120},
                new GameDto {Id = "2", Key = "grand2", Price = 120},
                new GameDto {Id = "2", Key = "grand3", Price = 120},
                new GameDto {Id = "2", Key = "grand3", Price = 120},
                new GameDto {Id = "2", Key = "grand3", Price = 150},
                new GameDto {Id = "2", Key = "grand3", Price = 1100},
                new GameDto {Id = "2", Key = "grand3", Price = 1550}
            };

            return sortedGames;
        }

        private static PageOptions CreatePageOptions()
        {
            var options = new PageOptions
            {
                PageNumber = 1,
                PageSize = TestPageSize,
                TotalItems = 11
            };

            return options;
        }

        private static GameFilterData CreateGameFilterData()
        {
            var filterData = new GameFilterData
            {
                PageSize = TestPageSize,
                PageNumber = 1,
                SortOption = SortOptions.PriceAsc,
            };

            return filterData;
        }
    }
}