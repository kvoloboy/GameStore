namespace GameStore.Infrastructure.DatabaseSettings.Interfaces
{
    public interface IMongoDatabaseSettings<in TEntity>
    {
        string GetDatabaseName();
        string GetCollectionName();
    }
}