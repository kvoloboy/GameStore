using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Factories.Interfaces;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Notification.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Core.Models.Notification;

namespace GameStore.BusinessLayer.Services.Notification
{
    public class OrderNotificationService : INotificationService<Order>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationSenderServiceFactory<Order> _notificationSenderServiceFactory;
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IAsyncReadonlyRepository<Core.Models.Notification.Notification> _notificationRepository;

        public OrderNotificationService(
            IUnitOfWork unitOfWork,
            INotificationSenderServiceFactory<Order> notificationSenderServiceFactory)
        {
            _unitOfWork = unitOfWork;
            _notificationSenderServiceFactory = notificationSenderServiceFactory;
            _userRepository = _unitOfWork.GetRepository<IAsyncRepository<User>>();
            _notificationRepository =
                _unitOfWork.GetRepository<IAsyncReadonlyRepository<Core.Models.Notification.Notification>>();
        }

        public async Task<IEnumerable<Core.Models.Notification.Notification>> GetNotificationMethodsAsync()
        {
            var notifications = await _notificationRepository.FindAllAsync();

            return notifications;
        }

        public async Task SubscribeAsync(string userId, IEnumerable<string> notificationsId)
        {
            if (notificationsId == null)
            {
                throw new InvalidServiceOperationException("Are null notifications");
            }
            
            var existingUser = await _userRepository.FindSingleAsync(u => u.Id == userId);

            if (existingUser == null)
            {
                throw new EntityNotFoundException<User>(userId);
            }

            existingUser.Notifications = notificationsId.Select(id => new UserNotification
            {
                UserId = userId,
                NotificationId = id
            }).ToList();

            await _userRepository.UpdateAsync(existingUser);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnsubscribeAsync(string userId)
        {
            var existingUser = await _userRepository.FindSingleAsync(u => u.Id == userId);

            if (existingUser == null)
            {
                throw new EntityNotFoundException<User>(userId);
            }

            existingUser.Notifications.Clear();

            await _userRepository.UpdateAsync(existingUser);
            await _unitOfWork.CommitAsync();
        }

        public async Task NotifyAsync(Order order)
        {
            await NotifyMailAsync(order);
        }

        private async Task NotifyMailAsync(Order order)
        {
            const string mailNotificationName = "E-mail";
            Expression<Func<User, bool>> predicate = u =>
                u.Notifications.Any(notification => notification.Notification.Name == mailNotificationName);

            var users = await _userRepository.FindAllAsync(predicate);

            var context = new OrderNotificationContext
            {
                Invoker = order,
                Subscribers = users
            };

            var mailNotificationService = _notificationSenderServiceFactory.Create(NotificationMethod.Email);

            await mailNotificationService.NotifyAsync(context);
        }
    }
}