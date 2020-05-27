using System;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class CommentControllerTests
    {
        private const string GameKey = "key";
        private const string ParentId = "1";

        private IMapper _mapper;
        private ILogger<GameController> _logger;
        private ICommentService _commentServices;
        private IAsyncViewModelFactory<CommentViewModel, DisplayCommentsViewModel> _displayCommentViewModelFactory;
        private CommentController _commentController;

        [SetUp]
        public void Setup()
        {
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<Logger<GameController>>();
            _commentServices = A.Fake<ICommentService>();
            _displayCommentViewModelFactory =
                A.Fake<IAsyncViewModelFactory<CommentViewModel, DisplayCommentsViewModel>>();
            _commentController =
                new CommentController(_commentServices, _displayCommentViewModelFactory, _mapper, _logger);
        }

        [Test]
        public void Create_ReturnsBadRequest_WhenEmptyKey()
        {
            var key = string.Empty;

            var result = _commentController.Create(key);

            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Test]
        public void Create_ReturnsPartialView_WhenValidKey()
        {
            var result = _commentController.Create(GameKey);

            result.Should().BePartialViewResult();
        }

        [Test]
        public void CreateAsync_ReturnsNotFound_WhenInvalidViewModel()
        {
            var testViewModel = CreateCommentViewModel();
            _commentController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _commentController.CreateAsync(testViewModel).Result;

            result.Should().BePartialViewResult();
        }

        [Test]
        public async Task ReplyAsync_ThrowsException_WhenNotFoundParent()
        {
            A.CallTo(() => _commentServices.GetByIdAsync(A<string>._))
                .Throws(new EntityNotFoundException<Comment>());

            Func<Task> action = async () => await _commentController.ReplyAsync(ParentId);

            await action.Should().ThrowAsync<EntityNotFoundException<Comment>>();
        }

        [Test]
        public void ReplyAsync_ReturnsPartialView_WhenFoundParent()
        {
            var commentDto = CreateCommentDto();
            A.CallTo(() => _commentServices.GetByIdAsync(A<string>._)).Returns(commentDto);

            var result = _commentController.ReplyAsync(ParentId).Result;

            result.Should().BePartialViewResult();
        }

        [Test]
        public void CreateAsync_ReturnsPartialView_WhenCreated()
        {
            var testViewModel = CreateCommentViewModel();

            var result = _commentController.CreateAsync(testViewModel).Result;

            result.Should().BePartialViewResult();
        }

        [Test]
        public void GetByGameKeyAsync_ReturnsNotFound_WhenEmptyKey()
        {
            var key = string.Empty;

            var result = _commentController.GetByGameKeyAsync(key).Result;

            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Test]
        public void GetByGameKeyAsync_ReturnsComments_WhenValidKey()
        {
            var result = _commentController.GetByGameKeyAsync(GameKey).Result;

            result.Should().BePartialViewResult();
        }

        [Test]
        public void DeleteAsync_ReturnsRedirect_Always()
        {
            const string id = "1";

            var result = _commentController.DeleteAsync(id).Result;

            result.Should().BeRedirectToActionResult();
        }

        private static CommentViewModel CreateCommentViewModel()
        {
            var comment = new CommentViewModel
            {
                Id = "2",
                Name = "Name",
                GameKey = GameKey,
                ParentId = "1"
            };

            return comment;
        }

        private static CommentDto CreateCommentDto()
        {
            var dto = new CommentDto
            {
                Id = "1",
                Name = "Name",
                GameKey = GameKey
            };

            return dto;
        }
    }
}