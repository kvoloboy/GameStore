using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.DataAccess.Sql.Context;
using GameStore.Infrastructure.Extensions;
using GameStore.Infrastructure.Logging.Interfaces;
using GameStore.Infrastructure.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Sql.Repositories
{
    public class UserAsyncRepository : IAsyncRepository<User>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UserAsyncRepository(AppDbContext dbContext, ILogger logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public Task<User> FindSingleAsync(Expression<Func<User, bool>> predicate)
        {
            var user = FindSingleAsync(predicate, false);

            return user;
        }

        public Task<List<User>> FindAllAsync(Expression<Func<User, bool>> predicate = null)
        {
            var users = _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .AsNoTracking();

            if (predicate != null)
            {
                users = users.Where(predicate);
            }

            return users.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
        {
            var any = _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .AnyAsync(predicate);

            return any;
        }

        public async Task AddAsync(User entity)
        {
            _dbContext.Users.Attach(entity);
            await _dbContext.Users.AddAsync(entity);

            var entry = new LogEntry<User>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(User entity)
        {
            if (entity.UserRoles != null)
            {
                foreach (var userRole in entity.UserRoles)
                {
                    userRole.Role = null;
                }
            }

            var existingUser = await FindSingleAsync(u => u.Id == entity.Id, true);
            var oldValueInstance = existingUser.Clone();

            _mapper.Map(entity, existingUser);

            var entry = new LogEntry<User>(Operation.Update, oldValueInstance, existingUser);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await FindSingleAsync(u => u.Id == id);
            user.IsDeleted = true;

            var entry = new LogEntry<User>(Operation.Delete, user);
            _logger.Log(entry);
        }

        private Task<User> FindSingleAsync(Expression<Func<User, bool>> predicate, bool isTracked)
        {
            var users = _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Include(u => u.Notifications)
                .IgnoreQueryFilters()
                .AsQueryable();
            users = isTracked ? users : users.AsNoTracking();

            var user = users.FirstOrDefaultAsync(predicate);

            return user;
        }
    }
}