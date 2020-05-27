using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Factories.Interfaces;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Notification;
using GameStore.BusinessLayer.Services.Notification.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Core.Models.Notification;
using NUnit.Framework;
using static FakeItEasy.A<GameStore.BusinessLayer.Models.NotificationContext<GameStore.Core.Models.Order>>;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class OrderNotificationServiceTests
    {
        private const string Id = "1";

        private IUnitOfWork _unitOfWork;
        private IAsyncRepository<User> _userRepository;
        private IAsyncReadonlyRepository<Notification> _notificationRepository;
        private INotificationSenderServiceFactory<Order> _notificationSenderServiceFactory;
        private INotificationSenderService<Order> _mailSender;

        private OrderNotificationService _orderNotificationService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userRepository = A.Fake<IAsyncRepository<User>>();
            _notificationRepository = A.Fake<IAsyncReadonlyRepository<Notification>>();
            _notificationSenderServiceFactory = A.Fake<INotificationSenderServiceFactory<Order>>();
            _mailSender = A.Fake<INotificationSenderService<Order>>();

            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<User>>())
                .Returns(_userRepository);
            
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncReadonlyRepository<Notification>>())
                .Returns(_notificationRepository);
            
            A.CallTo(() => _notificationSenderServiceFactory.Create(NotificationMethod.Email))
                .Returns(_mailSender);
            
            _orderNotificationService = new OrderNotificationService(_unitOfWork, _notificationSenderServiceFactory);
        }

        [Test]
        public void GetNotificationMethodsAsync_CallsRepository_Always()
        {
            _orderNotificationService.GetNotificationMethodsAsync();

            A.CallTo(() => _notificationRepository.FindAllAsync(A<Expression<Func<Notification, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task SubscribeAsync_ThrowsException_WhenNullNotificationsId()
        {
            Func<Task> action = async () => await _orderNotificationService.SubscribeAsync(Id, null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public async Task SubscribeAsync_ThrowsException_WhenUserNotFound()
        {
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns((User) null);

            Func<Task> action = async () =>
                await _orderNotificationService.SubscribeAsync(Id, Enumerable.Empty<string>());

            await action.Should().ThrowAsync<EntityNotFoundException<User>>();
        }

        [Test]
        public void SubscribeAsync_CallsRepository_WhenValidParams()
        {
            var user = GetUser();
            var notifications = new[] {"1"};
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns(user);
            Expression<Func<User, bool>> matchPredicate =
                u => u.Notifications.Any(n => n.NotificationId == notifications[0]);

            _orderNotificationService.SubscribeAsync(Id, notifications);

            A.CallTo(() => _userRepository.UpdateAsync(A<User>.That.Matches(matchPredicate)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UnsubscribeAsync_ThrowsException_WhenUserNotFound()
        {
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns((User) null);

            Func<Task> action = async () => await _orderNotificationService.UnsubscribeAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<User>>();
        }

        [Test]
        public void UnsubscribeAsync_ClearUserNotifications_WhenFound()
        {
            var user = GetUser();
            Expression<Func<User, bool>> matchPredicate = u => !u.Notifications.Any();
            A.CallTo(() => _userRepository.FindSingleAsync(A<Expression<Func<User, bool>>>._)).Returns(user);

            _orderNotificationService.UnsubscribeAsync(Id);

            A.CallTo(() => _userRepository.UpdateAsync(A<User>.That.Matches(matchPredicate)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void NotifyAsync_GetsSubscribedUsers_Always()
        {
            var order = new Order();

            _orderNotificationService.NotifyAsync(order);

            A.CallTo(() => _userRepository.FindAllAsync(A<Expression<Func<User, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void NotifyAsync_CreatesNotificationSender_Always()
        {
            var order = new Order();

            _orderNotificationService.NotifyAsync(order);

            A.CallTo(() => _notificationSenderServiceFactory.Create(NotificationMethod.Email))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void NotifyAsync_CallsNotificationSender_WhenCreated()
        {
            var order = new Order();

            _orderNotificationService.NotifyAsync(order);

            A.CallTo(() => _mailSender.NotifyAsync(That.Matches(context => context.Invoker == order)))
                .MustHaveHappenedOnceExactly();
        }

        private static User GetUser()
        {
            var user = new User
            {
                Id = Id,
                Notifications = new List<UserNotification>
                {
                    new UserNotification {NotificationId = "2", UserId = Id}
                }
            };

            return user;
        }
    }
}