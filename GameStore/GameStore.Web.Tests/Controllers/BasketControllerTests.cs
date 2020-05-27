using System.Collections.Generic;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class BasketControllerTests
    {
        private const string Id = "1";
        private const string CustomerId = "1";
        private const int Quantity = 2;

        private IBasketService _basketService;
        private IGameService _gameService;
        private IMapper _mapper;
        private ILogger<BasketController> _logger;
        private BasketController _basketController;

        [SetUp]
        public void Setup()
        {
            _basketService = A.Fake<IBasketService>();
            _gameService = A.Fake<IGameService>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<BasketController>>();

            _basketController = new BasketController(_basketService, _logger, _mapper, _gameService);
        }

        [Test]
        public void IndexAsync_ReturnsViewWithBasket_Always()
        {
            var basketViewModel = new BasketViewModel
            {
                OrderDetails = new List<OrderDetailsViewModel>()
            };
            A.CallTo(() => _mapper.Map<BasketViewModel>(A<Basket>._)).Returns(basketViewModel);
            
            var result = _basketController.IndexAsync().Result as ViewResult;

            result.Model.Should().BeAssignableTo<BasketViewModel>();
        }

        [Test]
        public void AddAsync_ReturnsBadRequest_WhenEmptyGameKey()
        {
            var gameKey = string.Empty;

            var result = _basketController.AddAsync(gameKey).Result;

            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Test]
        public void AddAsync_ReturnsRedirect_WhenCreated()
        {
            const string gameKey = "key";

            var result = _basketController.AddAsync(gameKey).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void UpdateQuantityAsync_CallsService_Always()
        {
            _basketController.UpdateQuantityAsync(Id, Quantity);

            A.CallTo(() => _basketService.UpdateQuantityAsync(Id, Quantity)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateQuantityAsync_ReturnsBadRequest_WhenExceptionWasThrown()
        {
            A.CallTo(() => _basketService.UpdateQuantityAsync(A<string>._, A<short>._))
                .Throws(new ValidationException<OrderDetails>(string.Empty, string.Empty));

            var result = _basketController.UpdateQuantityAsync(Id, Quantity).Result;

            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Test]
        public void UpdateQuantityAsync_ReturnsPartialView_WhenUpdated()
        {
            var result = _basketController.UpdateQuantityAsync(Id, Quantity).Result;

            result.Should().BePartialViewResult();
        }

        [Test]
        public void DeleteAsync_ReturnsPartialView_WhenDeleted()
        {
            var result = _basketController.DeleteAsync(CustomerId).Result;

            result.Should().BePartialViewResult();
        }
    }
}