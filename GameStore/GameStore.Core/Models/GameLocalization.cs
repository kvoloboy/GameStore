using System;

namespace GameStore.Core.Models
{
    public class GameLocalization
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string QuantityPerUnit { get; set; }

        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }

        public string CultureName { get; set; }
        public UserCulture UserCulture { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is GameLocalization localization))
            {
                return false;
            }

            var areEquals =
                Name == localization.Name &&
                Description == localization.Description &&
                QuantityPerUnit == localization.QuantityPerUnit;

            return areEquals;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(
                Id,
                Name,
                Description,
                QuantityPerUnit,
                CultureName);
        }
    }
}