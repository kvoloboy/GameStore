using System;
using System.Collections.Generic;

namespace GameStore.Core.Models
{
    public class Genre
    {
        public Genre()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public Genre Parent { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<GameGenre> GameGenres { get; set; }
    }
}