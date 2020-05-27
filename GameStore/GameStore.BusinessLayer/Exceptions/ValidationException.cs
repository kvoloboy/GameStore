using System;

namespace GameStore.BusinessLayer.Exceptions
{
    public class ValidationException<TEntity> : InvalidServiceOperationException
        where TEntity : class
    {
        public ValidationException(string value, string message)
            : base(message)
        {
            EntityType = typeof(TEntity);
            Value = value;
        }

        public ValidationException(string key, string value, string message)
            : this(value, message)
        {
            Key = key;
        }

        public Type EntityType { get; }
        public string Key { get; } = string.Empty;
        public string Value { get; }
    }
}