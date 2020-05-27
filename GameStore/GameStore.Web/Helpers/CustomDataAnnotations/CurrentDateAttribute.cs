using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Helpers.CustomDataAnnotations
{
    public class CurrentDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var date = (DateTime) value;
            
            return date.Month >= DateTime.UtcNow.Month;
        }
    }
}