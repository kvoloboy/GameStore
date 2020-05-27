using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class GenreServiceTests
    {
        private const string Id = "1";
        private const string GenreName = "FPS";

        private IUnitOfWork _unitOfWork;
        private IAsyncRepository<Genre> _genresRepository;
        private IMapper _mapper;
        private GenreService _genreServices;

        [SetUp]
        public void Setup()
        {
            _genresRepository = A.Fake<IAsyncRepository<Genre>>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<Genre>>()).Returns(_genresRepository);

            _genreServices = new GenreService(_unitOfWork, _mapper);
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _genreServices.CreateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is null genre dto");
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenExistsWithSameName()
        {
            var testGenreDto = CreateTestGenreDto(GenreName);
            A.CallTo(() => _genresRepository.AnyAsync(A<Expression<Func<Genre, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _genreServices.CreateAsync(testGenreDto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<Genre>>()
                .WithMessage($"Entity Genre with Name : {GenreName} already exists.");
        }

        [Test]
        public void CreateAsync_CallsRepository_WhenNotExistingGenreWithSameName()
        {
            var dto = CreateTestGenreDto(GenreName);

            _genreServices.CreateAsync(dto);

            A.CallTo(() => _genresRepository.AddAsync(A<Genre>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _genreServices.UpdateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is null genre dto");
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNotExistsWithId()
        {
            var dto = CreateTestGenreDto(GenreName);
            A.CallTo(() => _genresRepository.FindSingleAsync(A<Expression<Func<Genre, bool>>>._))
                .Returns((Genre) null);

            Func<Task> action = async () => await _genreServices.UpdateAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException<Genre>>()
                .WithMessage($"Entity Genre wasn't found. Id: {dto.Id}");
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenChangedNameAndNameExists()
        {
            var testGenreDto = CreateTestGenreDto("Name 1");
            var existingGenreDto = CreateTestGenreDto("Name 2");
            A.CallTo(() => _mapper.Map<GenreDto>(existingGenreDto)).Returns(testGenreDto);
            A.CallTo(() => _genresRepository.AnyAsync(A<Expression<Func<Genre, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _genreServices.UpdateAsync(testGenreDto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<Genre>>();
        }

        [Test]
        public void UpdateAsync_CallsRepository_WhenNotChangedName()
        {
            var testGenreDto = CreateTestGenreDto(GenreName);
            var testGenre = CreateTestGenre(GenreName);
            A.CallTo(() => _genresRepository.FindSingleAsync(A<Expression<Func<Genre, bool>>>._))
                .Returns(testGenre);

            _genreServices.UpdateAsync(testGenreDto);

            A.CallTo(() => _genresRepository.UpdateAsync(A<Genre>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DeleteAsync_ThrowsException_WhenNotExists()
        {
            A.CallTo(() => _genresRepository.FindSingleAsync(A<Expression<Func<Genre, bool>>>._))
                .Returns((Genre) null);

            Func<Task> action = async () => await _genreServices.DeleteAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Genre>>()
                .WithMessage($"Entity Genre wasn't found. Id: {Id}");
        }

        [Test]
        public void DeleteAsync_CallsRepository_WhenExistsWithId()
        {
            A.CallTo(() => _genresRepository.AnyAsync(A<Expression<Func<Genre, bool>>>._)).Returns(true);

            _genreServices.DeleteAsync(Id);

            A.CallTo(() => _genresRepository.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllAsync_ReturnsCollection_Always()
        {
            var genres = _genreServices.GetAllAsync();

            genres.Should().NotBeNull();
        }

        [Test]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            A.CallTo(() => _genresRepository.FindSingleAsync(A<Expression<Func<Genre, bool>>>._))
                .Returns((Genre) null);

            Func<Task> action = async () => await _genreServices.GetByIdAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Genre>>()
                .WithMessage($"Entity Genre wasn't found. Id: {Id}");
        }

        [Test]
        public void GetByIdAsync_ReturnsDto_WhenFound()
        {
            var genre = CreateTestGenre(GenreName);
            var expectedGenreDto = CreateTestGenreDto(GenreName);
            A.CallTo(() => _genresRepository.FindSingleAsync(A<Expression<Func<Genre, bool>>>._)).Returns(genre);
            A.CallTo(() => _mapper.Map<GenreDto>(genre)).Returns(expectedGenreDto);

            var dto = _genreServices.GetByIdAsync(Id).Result;

            dto.Should().BeEquivalentTo(expectedGenreDto);
        }

        [Test]
        public void GetGenresTreeAsync_ThrowsException_WhenNullArgument()
        {
            A.CallTo(() => _genresRepository.FindAllAsync(A<Expression<Func<Genre, bool>>>._))
                .Returns((List<Genre>) null);

            var tree = _genreServices.GetGenresTreeAsync();

            tree.Should().NotBeNull();
        }

        [Test]
        public void GetGenresTreeAsync_ReturnsGenreNodeDtoCollection_WhenValidArgument()
        {
            var testGenres = CreateTestCollection();
            var testNodes = CreateNodeCollection();
            var expectedTree = CreateExpectedGenreTree();
            A.CallTo(() => _genresRepository.FindAllAsync(A<Expression<Func<Genre, bool>>>._)).Returns(testGenres);
            A.CallTo(() => _mapper.Map<IEnumerable<GenreNodeDto>>(testGenres)).Returns(testNodes);

            var actualTree = _genreServices.GetGenresTreeAsync().Result;

            actualTree.Should().BeEquivalentTo(expectedTree);
        }

        private static Genre CreateTestGenre(string name, string id = "1")
        {
            var genre = new Genre
            {
                Id = id,
                Name = name
            };

            return genre;
        }

        private static GenreDto CreateTestGenreDto(string name, string id = "1")
        {
            var genreDto = new GenreDto
            {
                Id = id,
                Name = name
            };

            return genreDto;
        }

        private static List<Genre> CreateTestCollection()
        {
            var root = new Genre
            {
                Id = "1",
                Name = "Root"
            };

            var subRoot = new Genre
            {
                Id = "2",
                Name = "Sub root",
                Parent = root
            };

            var leaf = new Genre
            {
                Id = "3",
                Name = "leaf",
                Parent = subRoot
            };

            return new List<Genre>
            {
                root, subRoot, leaf
            };
        }

        private static IEnumerable<GenreNodeDto> CreateNodeCollection()
        {
            var root = new GenreNodeDto
            {
                Id = "1",
                Name = "Root"
            };

            var subRoot = new GenreNodeDto
            {
                Id = "2",
                Name = "Sub root",
                ParentId = "1"
            };

            var leaf = new GenreNodeDto
            {
                Id = "3",
                Name = "leaf",
                ParentId = "2"
            };

            return new List<GenreNodeDto>
            {
                root, subRoot, leaf
            };
        }

        private static IEnumerable<GenreNodeDto> CreateExpectedGenreTree()
        {
            var leaf = new GenreNodeDto
            {
                Id = "3",
                Name = "leaf",
                ParentId = "2",
                Children = new List<GenreNodeDto>()
            };

            var subRoot = new GenreNodeDto
            {
                Id = "2",
                Name = "Sub root",
                ParentId = "1",
                Children = new List<GenreNodeDto>
                {
                    leaf
                }
            };

            var root = new GenreNodeDto
            {
                Id = "1",
                Name = "Root",
                Children = new List<GenreNodeDto>
                {
                    subRoot
                }
            };

            return new List<GenreNodeDto>
            {
                root
            };
        }
    }
}