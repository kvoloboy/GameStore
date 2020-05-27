using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Core.Abstractions;
using GameStore.Core.Models.Notification;
using GameStore.DataAccess.Sql.Context;
using GameStore.Infrastructure.Logging.Interfaces;
using GameStore.Infrastructure.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Sql.Repositories
{
    public class NotificationAsyncRepository : IAsyncReadonlyRepository<Notification>
    {
        private readonly AppDbContext _dbContext;

        public NotificationAsyncRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Notification> FindSingleAsync(Expression<Func<Notification, bool>> predicate)
        {
            var notification = _dbContext.Notifications.FirstOrDefaultAsync(predicate);

            return notification;
        }

        public Task<List<Notification>> FindAllAsync(Expression<Func<Notification, bool>> predicate = null)
        {
            var notifications = _dbContext.Notifications.AsNoTracking();

            if (predicate != null)
            {
                notifications = notifications.Where(predicate);
            }

            return notifications.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Notification, bool>> predicate)
        {
            var any = _dbContext.Notifications.AnyAsync(predicate);

            return any;
        }
    }
}