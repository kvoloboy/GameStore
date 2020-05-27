using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Helpers.CustomDataAnnotations
{
    public class CvvAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            const int cvvLength = 3;
            var cvv = value?.ToString() ?? string.Empty;
            
            return cvv.Length == cvvLength && int.TryParse(cvv, out _);
        }
    }
}