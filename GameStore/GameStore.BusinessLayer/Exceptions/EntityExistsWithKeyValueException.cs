namespace GameStore.BusinessLayer.Exceptions
{
    public class EntityExistsWithKeyValueException<TEntity> : ValidationException<TEntity>
        where TEntity : class
    {
        public EntityExistsWithKeyValueException(string key, string value)
            : base(key, value, FormMessage(key, value))
        {
        }

        private static string FormMessage(string key, string value)
        {
            var message = $"Entity {typeof(TEntity).Name} with {key} : {value} already exists.";

            return message;
        }
    }
}