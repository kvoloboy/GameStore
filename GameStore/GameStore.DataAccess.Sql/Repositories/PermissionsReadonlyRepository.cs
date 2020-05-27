using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Core.Abstractions;
using GameStore.Core.Models.Identity;
using GameStore.DataAccess.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Sql.Repositories
{
    public class PermissionsReadonlyRepository : IAsyncReadonlyRepository<Permission>
    {
        private readonly AppDbContext _dbContext;

        public PermissionsReadonlyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Permission> FindSingleAsync(Expression<Func<Permission, bool>> predicate)
        {
            var permission = _dbContext.Permissions.FirstOrDefaultAsync(predicate);

            return permission;
        }

        public Task<List<Permission>> FindAllAsync(Expression<Func<Permission, bool>> predicate = null)
        {
            var permissions = _dbContext.Permissions.AsNoTracking();

            if (predicate != null)
            {
                permissions = permissions.Where(predicate);
            }

            return permissions.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Permission, bool>> predicate)
        {
            var any = _dbContext.Permissions.AnyAsync(predicate);

            return any;
        }
    }
}