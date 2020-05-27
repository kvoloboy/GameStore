using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Identity.Factories.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels;
using GameStore.Web.Models.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private const string Id = "1";

        private IUserService _userService;
        private IClaimsPrincipalFactory _claimsPrincipalFactory;
        private ILogger<AccountController> _logger;
        private IStringLocalizer<AccountController> _stringLocalizer;

        private AccountController _accountController;

        [SetUp]
        public void Setup()
        {
            _userService = A.Fake<IUserService>();
            _claimsPrincipalFactory = A.Fake<IClaimsPrincipalFactory>();
            _logger = A.Fake<ILogger<AccountController>>();
            _stringLocalizer = A.Fake<IStringLocalizer<AccountController>>();

            _accountController = new AccountController(
                _userService,
                _claimsPrincipalFactory,
                _logger,
                _stringLocalizer);
        }

        [Test]
        public void Ban_ReturnsView_Always()
        {
            var result = _accountController.Ban(Id) as ViewResult;
            var model = result.Model as BanViewModel;

            model.UserId.Should().Be(Id);
        }

        [Test]
        public void Ban_CallsService_Always()
        {
            _accountController.BanAsync(Id, BanTerm.Day);

            A.CallTo(() => _userService.BanAsync(Id, BanTerm.Day)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void Register_ReturnView_Always()
        {
            var result = _accountController.Register() as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<RegisterViewModel>();
        }

        [Test]
        public void SignIn_ReturnsView_Always()
        {
            var result = _accountController.SignIn() as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<SignInViewModel>();
        }
    }
}