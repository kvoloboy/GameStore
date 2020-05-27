using GameStore.Infrastructure.Logging.Models;

namespace GameStore.Infrastructure.Logging.Interfaces
{
    public interface ILogger
    {
        void Log<TEntity>(LogEntry<TEntity> logEntry);
    }
}