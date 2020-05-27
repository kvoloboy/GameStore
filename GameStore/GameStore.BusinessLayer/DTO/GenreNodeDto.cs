using System.Collections.Generic;

namespace GameStore.BusinessLayer.DTO
{
    public class GenreNodeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public IEnumerable<GenreNodeDto> Children { get; set; }
    }
}