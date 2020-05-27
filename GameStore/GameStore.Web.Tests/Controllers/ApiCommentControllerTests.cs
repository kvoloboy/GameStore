using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class ApiCommentControllerTests
    {
        private const string Id = "1";
        
        private ICommentService _commentService;
        private ILogger<ApiCommentController> _logger;
        private IMapper _mapper;
        private ApiCommentController _apiCommentController;

        [SetUp]
        public void Setup()
        {
            _commentService = A.Fake<ICommentService>();
            _logger = A.Fake<ILogger<ApiCommentController>>();
            _mapper = A.Fake<IMapper>();

            _apiCommentController = new ApiCommentController(_commentService, _logger, _mapper);
        }

        [Test]
        public void CreateAsync_CallsService_Always()
        {
            var viewModel = GetCommentViewModel();
            
            _apiCommentController.CreateAsync(viewModel);

            A.CallTo(() => _commentService.CreateAsync(A<CommentDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CreateAsync_ReturnsCreatedResult_WhenValidViewModel()
        {
            var viewModel = GetCommentViewModel();

            var result = _apiCommentController.CreateAsync(viewModel).Result as CreatedAtActionResult;
            
            result.Value.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void UpdateAsync_CallsService_Always()
        {
            var viewModel = GetCommentViewModel();
            
            _apiCommentController.UpdateAsync(viewModel);

            A.CallTo(() => _commentService.UpdateAsync(A<CommentDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_ReturnsNoContent_WhenValidViewModel()
        {
            var viewModel = GetCommentViewModel();

            var result = _apiCommentController.UpdateAsync(viewModel).Result;
            
            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void DeleteAsync_CallsService_Always()
        {
            _apiCommentController.DeleteAsync(Id);

            A.CallTo(() => _commentService.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void DeleteAsync_ReturnsNoContent_WhenDeleted()
        {
            var result = _apiCommentController.DeleteAsync(Id).Result;
            
            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void GetByIdAsync_ReturnsViewModel_WhenFound()
        {
            var result = _apiCommentController.GetByIdAsync(Id);

            result.Should().NotBeNull();
        }

        private static CommentViewModel GetCommentViewModel()
        {
            var viewModel = new CommentViewModel
            {
                Id = Id,
                Name = "Name"
            };

            return viewModel;
        }
    }
}