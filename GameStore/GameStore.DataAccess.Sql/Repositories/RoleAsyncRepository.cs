using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Core.Abstractions;
using GameStore.Core.Models.Identity;
using GameStore.DataAccess.Sql.Context;
using GameStore.Infrastructure.Extensions;
using GameStore.Infrastructure.Logging.Interfaces;
using GameStore.Infrastructure.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Sql.Repositories
{
    public class RoleAsyncRepository : IAsyncRepository<Role>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public RoleAsyncRepository(AppDbContext dbContext, ILogger logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public Task<Role> FindSingleAsync(Expression<Func<Role, bool>> predicate)
        {
            var role = FindSingleAsync(predicate, false);

            return role;
        }

        public Task<List<Role>> FindAllAsync(Expression<Func<Role, bool>> predicate = null)
        {
            var roles = _dbContext.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .AsNoTracking();

            if (predicate != null)
            {
                roles = roles.Where(predicate);
            }

            return roles.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Role, bool>> predicate)
        {
            var any = _dbContext.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .AnyAsync(predicate);

            return any;
        }

        public async Task AddAsync(Role entity)
        {
            _dbContext.Roles.Attach(entity);
            await _dbContext.Roles.AddAsync(entity);

            var entry = new LogEntry<Role>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(Role entity)
        {
            var existingRole = await FindSingleAsync(r => r.Id == entity.Id, true);
            var oldValueInstance = existingRole.Clone();
            _mapper.Map(entity, existingRole);

            var entry = new LogEntry<Role>(Operation.Update, oldValueInstance, existingRole);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var role = await FindSingleAsync(r => r.Id == id, true);
            _dbContext.Roles.Remove(role);

            var entry = new LogEntry<Role>(Operation.Delete, role);
            _logger.Log(entry);
        }

        private Task<Role> FindSingleAsync(Expression<Func<Role, bool>> predicate, bool isTracked)
        {
            var roles = isTracked ? _dbContext.Roles : _dbContext.Roles.AsNoTracking();
            var role = roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(predicate);

            return role;
        }
    }
}