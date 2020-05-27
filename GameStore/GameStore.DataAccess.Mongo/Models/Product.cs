using System;

namespace GameStore.DataAccess.Mongo.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Key { get; set; }
        public string SupplierId { get; set; }
        public string QuantityPerUnit { get; set; }
        public bool Discontinued { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public int UnitOnOrder { get; set; }

        public override bool Equals(object other)
        {
            if (!(other is Product product))
            {
                return false;
            }

            return Id == product.Id &&
                   ProductName == product.ProductName &&
                   Key == product.Key &&
                   QuantityPerUnit == product.QuantityPerUnit &&
                   Discontinued == product.Discontinued &&
                   UnitPrice == product.UnitPrice &&
                   UnitsInStock == product.UnitsInStock &&
                   UnitOnOrder == product.UnitOnOrder;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            hashCode.Add(Id);
            hashCode.Add(ProductName);
            hashCode.Add(Key);
            hashCode.Add(SupplierId);
            hashCode.Add(QuantityPerUnit);
            hashCode.Add(Discontinued);
            hashCode.Add(UnitPrice);
            hashCode.Add(UnitsInStock);
            hashCode.Add(UnitOnOrder);

            return hashCode.ToHashCode();
        }
    }
}