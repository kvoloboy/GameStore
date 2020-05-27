﻿ using MongoDB.Bson;

  namespace GameStore.SeedingServices.Models
{
    public class Product
    {
        public ObjectId Id { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string Key { get; set; }
    }
}