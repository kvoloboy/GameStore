using System.Collections.Generic;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.RoleViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class RoleControllerTests
    {
        private const string Id = "1";

        private IRoleService _roleService;
        private IAsyncViewModelFactory<ModifyRoleViewModel, ModifyRoleViewModel> _modifyRoleViewModelFactory;
        private IMapper _mapper;
        private ILogger<RoleController> _logger;
        private RoleController _roleController;

        [SetUp]
        public void Setup()
        {
            _roleService = A.Fake<IRoleService>();
            _modifyRoleViewModelFactory = A.Fake<IAsyncViewModelFactory<ModifyRoleViewModel, ModifyRoleViewModel>>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<RoleController>>();
            _roleController = new RoleController(_modifyRoleViewModelFactory, _roleService, _mapper, _logger);
        }

        [Test]
        public void IndexAsync_ReturnsView_Always()
        {
            var result = _roleController.IndexAsync().Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<IEnumerable<RoleViewModel>>();
        }

        [Test]
        public void CreateAsync_ReturnsViewWithAvailablePermissions_Always()
        {
            var result = _roleController.CreateAsync().Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<ModifyRoleViewModel>();
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenModelStateIsInvalid()
        {
            var viewModel = CreateModifyRoleViewModel();
            _roleController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _roleController.CreateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenCatchException()
        {
            var viewModel = CreateModifyRoleViewModel();

            A.CallTo(() => _roleService.CreateAsync(A<RoleDto>._))
                .Throws(new InvalidServiceOperationException(string.Empty));

            var result = _roleController.CreateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void CreateAsync_ReturnsRedirect_WhenRoleIsCreated()
        {
            var viewModel = CreateModifyRoleViewModel();

            var result = _roleController.CreateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }
        
        [Test]
        public void UpdateAsync_ReturnsViewWithAvailablePermissions_Always()
        {
            var result = _roleController.UpdateAsync(Id).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<ModifyRoleViewModel>();
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenModelStateIsInvalid()
        {
            var viewModel = CreateModifyRoleViewModel();
            _roleController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _roleController.UpdateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenCatchException()
        {
            var viewModel = CreateModifyRoleViewModel();
            A.CallTo(() => _roleService.UpdateAsync(A<RoleDto>._))
                .Throws(new InvalidServiceOperationException(string.Empty));

            var result = _roleController.UpdateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void UpdateAsync_ReturnsRedirect_WhenRoleIsCreated()
        {
            var viewModel = CreateModifyRoleViewModel();

            var result = _roleController.UpdateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }
        
        [Test]
        public void DeleteAsync_CallsService_Always()
        {
            _roleController.DeleteAsync(Id);

            A.CallTo(() => _roleService.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        private static ModifyRoleViewModel CreateModifyRoleViewModel()
        {
            var viewModel = new ModifyRoleViewModel();

            return viewModel;
        }
    }
}