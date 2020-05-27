using System;

namespace GameStore.BusinessLayer.Exceptions
{
    public class BaseEntityNotFoundException : Exception
    {
        public BaseEntityNotFoundException(string id = null, Exception innerException = null)
            : base(id, innerException)
        {
        }

        public string EntityId { get; set; }
    }
}