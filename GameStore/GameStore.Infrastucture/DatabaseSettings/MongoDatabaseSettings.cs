using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GameStore.Infrastructure.DatabaseSettings
{
    public class MongoDatabaseSettings<TEntity> : IMongoDatabaseSettings<TEntity>
    {
        private const string ConfigSegmentName = "MongoDatabaseSettings";
        private readonly IConfiguration _configuration;

        public MongoDatabaseSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetDatabaseName()
        {
            const string databaseNameSegment = "DatabaseName";
            var databaseName = _configuration[$"{ConfigSegmentName}:{databaseNameSegment}"];

            return databaseName;
        }

        public string GetCollectionName()
        {
            const string collectionsSegment = "Collections";
            var targetCollectionKey = typeof(TEntity).Name;
            var targetCollectionValue =
                _configuration[$"{ConfigSegmentName}:{collectionsSegment}:{targetCollectionKey}"];

            return targetCollectionValue;
        }
    }
}