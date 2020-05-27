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
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class CommentServiceTests
    {
        private const string Id = "1";
        private const string Key = "key";

        private IUnitOfWork _unitOfWork;
        private IAsyncRepository<Comment> _commentsRepository;
        private IGameService _gameService;
        private IMapper _mapper;
        private CommentService _commentServices;

        [SetUp]
        public void Setup()
        {
            _mapper = A.Fake<IMapper>();
            _commentsRepository = A.Fake<IAsyncRepository<Comment>>();
            _gameService = A.Fake<IGameService>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<Comment>>()).Returns(_commentsRepository);
            _commentServices = new CommentService(_unitOfWork, _gameService, _mapper);
        }

        [Test]
        public async Task GetCommentsTreeByGameKeyAsync_ThrowsException_WhenEmptyKey()
        {
            var key = string.Empty;

            Func<Task> action = async () => await _commentServices.GetCommentsTreeByGameKeyAsync(key);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is not valid key");
        }

        [Test]
        public void GetCommentsTreeByGameKeyAsync_ReturnsCollection_WhenFound()
        {
            A.CallTo(() => _commentsRepository.FindAllAsync(A<Expression<Func<Comment, bool>>>._))
                .Returns(new List<Comment> {new Comment()});

            var comments = _commentServices.GetCommentsTreeByGameKeyAsync(Key);

            comments.Should().NotBeNull();
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _commentServices.CreateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is null comment dto");
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenEmptyGameKey()
        {
            var dto = CreateCommentDto();
            var comment = CreateComment();
            A.CallTo(() => _mapper.Map<Comment>(dto)).Returns(comment);

            Func<Task> action = async () => await _commentServices.CreateAsync(dto);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is empty game key");
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenEmptyUserId()
        {
            var dto = CreateCommentDto(Key);
            var comment = CreateComment();
            A.CallTo(() => _mapper.Map<Comment>(dto)).Returns(comment);

            Func<Task> action = async () => await _commentServices.CreateAsync(dto);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void CreateAsync_CallsRepository_WhenValidDto()
        {
            var dto = CreateCommentDto(Key, Id);
            var comment = CreateComment();
            A.CallTo(() => _mapper.Map<Comment>(dto)).Returns(comment);

            _commentServices.CreateAsync(dto);

            A.CallTo(() => _commentsRepository.AddAsync(A<Comment>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DeleteAsync_ThrowsException_WhenNotExist()
        {
            Func<Task> action = async () => await _commentServices.DeleteAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Comment>>();
        }

        [Test]
        public void DeleteAsync_CallsRepository_WhenExists()
        {
            A.CallTo(() => _commentsRepository.AnyAsync(A<Expression<Func<Comment, bool>>>._)).Returns(true);

            _commentServices.DeleteAsync(Id);

            A.CallTo(() => _commentsRepository.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetCommentsTreeByGameKeyAsync_ThrowException_WhenEmptyGameKey()
        {
            var key = string.Empty;

            Func<Task> action = async () => await _commentServices.GetCommentsTreeByGameKeyAsync(key);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is not valid key");
        }

        [Test]
        public void GetCommentsTreeByGameKeyAsync_CommentsDtoCollection_WhenValidKey()
        {
            var inputData = CreateTestCollection();
            var commentsDto = CreateCommentsDtoCollection();
            var expectedTree = CreateExpectedCommentTree();
            A.CallTo(() => _commentsRepository.FindAllAsync(A<Expression<Func<Comment, bool>>>._))
                .Returns(inputData);
            A.CallTo(() => _mapper.Map<IEnumerable<CommentDto>>(inputData)).Returns(commentsDto);

            var actualTree = _commentServices.GetCommentsTreeByGameKeyAsync(Key).Result;

            actualTree.Should().BeEquivalentTo(expectedTree);
        }

        [Test]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            A.CallTo(() => _commentsRepository.FindSingleAsync(A<Expression<Func<Comment, bool>>>._))
                .Returns((Comment) null);

            Func<Task> action = async () => await _commentServices.GetByIdAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Comment>>();
        }

        [Test]
        public void GetByIdAsync_ReturnsDto_WhenExists()
        {
            var dto = _commentServices.GetByIdAsync(Id);

            dto.Should().NotBeNull();
        }

        [Test]
        public async Task UpdateCommentsOwnerAsync_ThrowsException_WhenEmptyOldUserId()
        {
            Func<Task> action = async () => await _commentServices.UpdateCommentsOwnerAsync(null, Id);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public async Task UpdateCommentsOwnerAsync_ThrowsException_WhenEmptyNewUserId()
        {
            Func<Task> action = async () => await _commentServices.UpdateCommentsOwnerAsync(Id, null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void UpdateCommentsOwnerAsync_SetsNewUserIdToComments_WhenValidArguments()
        {
            const string newId = "2";
            const int commentsCount = 3;
            var testComments = CreateTestCollection();
            A.CallTo(() => _commentsRepository.FindAllAsync(A<Expression<Func<Comment, bool>>>._))
                .Returns(testComments);

            _commentServices.UpdateCommentsOwnerAsync(Id, newId);

            A.CallTo(() => _commentsRepository.UpdateAsync(A<Comment>.That.Matches(c => c.UserId == newId)))
                .MustHaveHappened(commentsCount, Times.Exactly);
        }

        private static CommentDto CreateCommentDto(string gameKey = null, string userId = null)
        {
            var dto = new CommentDto
            {
                GameKey = gameKey,
                UserId = userId
            };

            return dto;
        }

        private static Comment CreateComment()
        {
            var comment = new Comment
            {
                GameRoot = new GameRoot
                {
                    Details = new GameDetails()
                }
            };

            return comment;
        }

        private static List<Comment> CreateTestCollection()
        {
            var root = new Comment
            {
                Id = "1",
                Name = "Andrew",
                Body = "Hi, how are you",
                UserId = Id
            };

            var subRoot = new Comment
            {
                Id = "2",
                Name = "Bob",
                Body = "Fine, you?",
                Parent = root,
                UserId = Id
            };

            var leaf = new Comment
            {
                Id = "3",
                Name = "John",
                Body = "What`s up, guys",
                Parent = subRoot,
                UserId = Id
            };

            return new List<Comment>
            {
                root, subRoot, leaf
            };
        }

        private static IEnumerable<CommentDto> CreateCommentsDtoCollection()
        {
            var root = new CommentDto
            {
                Id = "1",
                Name = "Andrew",
                Body = "Hi, how are you"
            };

            var subRoot = new CommentDto
            {
                Id = "2",
                Name = "Bob",
                Body = "Fine, you?",
                ParentId = root.Id
            };

            var leaf = new CommentDto
            {
                Id = "3",
                Name = "John",
                Body = "What`s up, guys",
                ParentId = subRoot.Id
            };

            return new List<CommentDto>
            {
                root, subRoot, leaf
            };
        }

        private static IEnumerable<CommentDto> CreateExpectedCommentTree()
        {
            var leaf = new CommentDto
            {
                Id = "3",
                Name = "John",
                Body = "What`s up, guys",
                ParentId = "2",
                Children = new List<CommentDto>()
            };

            var subRoot = new CommentDto
            {
                Id = "2",
                Name = "Bob",
                Body = "Fine, you?",
                ParentId = "1",
                Children = new List<CommentDto>
                {
                    leaf
                }
            };

            var root = new CommentDto
            {
                Id = "1",
                Name = "Andrew",
                Body = "Hi, how are you",
                Children = new List<CommentDto>
                {
                    subRoot
                }
            };

            return new List<CommentDto>
            {
                root
            };
        }
    }
}