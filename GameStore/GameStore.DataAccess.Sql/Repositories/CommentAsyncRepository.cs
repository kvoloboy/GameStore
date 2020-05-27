using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.DataAccess.Sql.Context;
using GameStore.Infrastructure.Extensions;
using GameStore.Infrastructure.Logging.Interfaces;
using GameStore.Infrastructure.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Sql.Repositories
{
    public class CommentRepository : IAsyncRepository<Comment>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public CommentRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(Comment entity)
        {
            entity.GameRoot = null;
            _dbContext.Attach(entity);
            await _dbContext.Comments.AddAsync(entity);

            var entry = new LogEntry<Comment>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(Comment entity)
        {
            var existingComment = await FindSingleAsync(c => c.Id == entity.Id);
            var oldValueInstance = existingComment.Clone();
            _dbContext.Entry(existingComment).CurrentValues.SetValues(entity);

            var entry = new LogEntry<Comment>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var existingComment = await FindSingleAsync(c => c.Id == id);
            existingComment.IsDeleted = true;

            var entry = new LogEntry<Comment>(Operation.Delete, existingComment);
            _logger.Log(entry);
        }

        public Task<Comment> FindSingleAsync(Expression<Func<Comment, bool>> filter)
        {
            var comment = _dbContext.Comments
                .Include(c => c.GameRoot)
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(filter);

            return comment;
        }

        public Task<List<Comment>> FindAllAsync(Expression<Func<Comment, bool>> predicate = null)
        {
            var comments = _dbContext.Comments
                .Include(c => c.GameRoot)
                .Include(c => c.Parent)
                .AsNoTracking();

            if (predicate != null)
            {
                comments = comments.Where(predicate);
            }

            return comments.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Comment, bool>> predicate)
        {
            var exists = _dbContext.Comments.AnyAsync(predicate);

            return exists;
        }
    }
}