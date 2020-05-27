using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.PaymentViewModels;
using GameStore.Web.WebServices.Payments.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private IPaymentStrategyFactory _paymentStrategyFactory;
        private IOrderService _orderService;
        private IPaymentStrategy _paymentStrategy;
        private PaymentController _paymentController;

        [SetUp]
        public void Setup()
        {
            _paymentStrategyFactory = A.Fake<IPaymentStrategyFactory>();
            _orderService = A.Fake<IOrderService>();
            _paymentStrategy = A.Fake<IPaymentStrategy>();
            A.CallTo(() => _paymentStrategyFactory.Create(A<string>._)).Returns(_paymentStrategy);
            _paymentController = new PaymentController(_paymentStrategyFactory, _orderService);
        }

        [Test]
        public void CheckoutAsync_ReturnsBadRequest_WhenEmptyPaymentName()
        {
            var payment = string.Empty;

            var result = _paymentController.CheckoutAsync(payment).Result;

            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Test]
        public void CheckoutAsync_CallsPaymentStrategy_Always()
        {
            const string payment = "payment";

            _paymentController.CheckoutAsync(payment);

            A.CallTo(() => _paymentStrategy.Pay(A<OrderDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void VisaCheckout_ReturnsView_WhenInvalidModelState()
        {
            var viewModel = new VisaViewModel();
            _paymentController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _paymentController.VisaCheckout(viewModel);

            result.Should().BeViewResult();
        }

        [Test]
        public void VisaCheckout_ConfirmsOrder_WhenValidState()
        {
            var viewModel = new VisaViewModel();

            _paymentController.VisaCheckout(viewModel);

            A.CallTo(() => _orderService.ConfirmAsync(A<string>._)).MustHaveHappenedOnceExactly();
        }
    }
}