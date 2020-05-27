using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.Services;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class PaymentTypeServiceTests
    {
        private IAsyncRepository<PaymentType> _paymentRepository;
        private PaymentTypeService _paymentTypeService;

        [SetUp]
        public void Setup()
        {
            _paymentRepository = A.Fake<IAsyncRepository<PaymentType>>();
            _paymentTypeService = new PaymentTypeService(_paymentRepository);
        }

        [Test]
        public void GetAllAsync_ReturnsPaymentTypes_Always()
        {
            var testPayments = CreateTestCollection();
            A.CallTo(() => _paymentRepository.FindAllAsync(A<Expression<Func<PaymentType, bool>>>._)).Returns(testPayments);
            var payments = _paymentTypeService.GetAllAsync().Result;

            payments.Count().Should().Be(testPayments.Count());
        }

        private static List<PaymentType> CreateTestCollection()
        {
            var payments = new[]
            {
                new PaymentType(),
                new PaymentType()
            };

            return payments.ToList();
        }
    }
}