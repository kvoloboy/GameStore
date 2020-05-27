using System;

namespace GameStore.Core.Models
{
    public class GameDetails
    {
        public GameDetails()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public short? UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public bool IsDiscontinued { get; set; }
        public DateTime CreationDate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is GameDetails gameDetails))
            {
                return false;
            }

            return Price == gameDetails.Price &&
                   Discount == gameDetails.Discount &&
                   UnitsInStock == gameDetails.UnitsInStock &&
                   UnitsOnOrder == gameDetails.UnitsOnOrder &&
                   IsDiscontinued == gameDetails.IsDiscontinued;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(GameRootId);
            hashCode.Add(GameRoot);
            hashCode.Add(Price);
            hashCode.Add(Discount);
            hashCode.Add(UnitsInStock);
            hashCode.Add(UnitsOnOrder);
            hashCode.Add(IsDiscontinued);
            hashCode.Add(CreationDate);

            return hashCode.ToHashCode();
        }
    }
}