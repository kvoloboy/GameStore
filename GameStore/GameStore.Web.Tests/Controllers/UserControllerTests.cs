using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.BusinessLayer.Services.Notification.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private const string Id = "1";

        private IUserService _userService;
        private IPublisherService _publisherService;
        private INotificationService<Order> _notificationService;
        private IAsyncViewModelFactory<string, ModifyUserViewModel> _modifyUserViewModelFactory;
        private ILogger<UserController> _logger;

        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _userService = A.Fake<IUserService>();
            _publisherService = A.Fake<IPublisherService>();
            _notificationService = A.Fake<INotificationService<Order>>();
            _modifyUserViewModelFactory = A.Fake<IAsyncViewModelFactory<string, ModifyUserViewModel>>();
            _logger = A.Fake<ILogger<UserController>>();

            _userController = new UserController(
                _userService,
                _notificationService,
                _publisherService,
                _modifyUserViewModelFactory,
                _logger);
        }

        [Test]
        public void IndexAsync_ReturnsViewWithUserList_Always()
        {
            var result = _userController.IndexAsync().Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<IEnumerable<UserViewModel>>();
        }

        [Test]
        public void SetupRolesAsync_ReturnsView_Always()
        {
            var result = _userController.SetupRolesAsync(Id).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void SetupRolesAsync_ReturnsBadRequest_WhenModelStateIsNotValid()
        {
            var viewModel = CreateModifyUserViewModel();
            _userController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _userController.SetupRolesAsync(viewModel).Result;

            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Test]
        public void SetupRolesAsync_CallsService_WhenValid()
        {
            var viewModel = CreateModifyUserViewModel();

            _userController.SetupRolesAsync(viewModel);

            A.CallTo(() => _userService.UpdateRolesAsync(viewModel.Id, viewModel.SelectedRoles))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void AssignToPublisherAsync_ReturnsViewWithPublishers_Always()
        {
            var result = _userController.AssignToPublisherAsync(Id).Result as ViewResult;
            var model = result.Model as AssignToPublisherViewModel;

            model.Publishers.Should().NotBeNull();
        }

        [Test]
        public void AssignToPublisherAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var viewModel = new AssignToPublisherViewModel();
            _userController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _userController.AssignToPublisherAsync(viewModel).Result;

            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Test]
        public void AssignToPublisherAsync_ReturnsRedirect_WhenAssigned()
        {
            var viewModel = CreateAssignToPublisherViewModel();

            _userController.AssignToPublisherAsync(viewModel);

            A.CallTo(() => _userService.AssignToPublisherAsync(viewModel.UserId, viewModel.PublisherId))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SubscribeOnNotificationsAsync_GetsAvailableNotificationMethods_Always()
        {
            _userController.SubscribeOnNotificationsAsync();

            A.CallTo(() => _notificationService.GetNotificationMethodsAsync()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SubscribeOnNotificationsAsync_ReturnsViewWithAssignedNotificationMethods_Always()
        {
            var result = _userController.SubscribeOnNotificationsAsync().Result as ViewResult;
            var model = result.Model as SubscribeOnNotificationsViewModel;

            model.NotificationMethods.Should().NotBeNull();
        }

        [Test]
        public void SubscribeOnNotificationsAsync_CallsService_Always()
        {
            _userController.SubscribeOnNotificationsAsync(Enumerable.Empty<string>());

            A.CallTo(() => _notificationService.SubscribeAsync(A<string>._, A<IEnumerable<string>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SubscribeOnNotificationsAsync_ReturnsRedirect_WhenSubscribed()
        {
            var result = _userController.SubscribeOnNotificationsAsync(Enumerable.Empty<string>()).Result;

            result.Should().BeRedirectToActionResult();
        }

        private static ModifyUserViewModel CreateModifyUserViewModel()
        {
            var viewModel = new ModifyUserViewModel
            {
                Id = Id,
                SelectedRoles = new[] {Id}
            };

            return viewModel;
        }

        private static AssignToPublisherViewModel CreateAssignToPublisherViewModel()
        {
            var viewModel = new AssignToPublisherViewModel
            {
                UserId = Id,
                PublisherId = Id
            };

            return viewModel;
        }
    }
}