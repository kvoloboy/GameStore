using System;

namespace GameStore.BusinessLayer.Exceptions
{
    public class EntityNotFoundException<TEntity> : BaseEntityNotFoundException
        where TEntity : class
    {
        public EntityNotFoundException(string id = null, Exception innerException = null)
            : base(
                FormMessage(id),
                innerException)
        {
            EntityId = id;
            EntityType = typeof(TEntity);
        }

        public Type EntityType { get; }

        private static string FormMessage(string id)
        {
            var message =
                $"Entity {typeof(TEntity).Name} wasn't found." +
                (!string.IsNullOrEmpty(id) ? $" Id: {id}" : string.Empty);

            return message;
        }
    }
}