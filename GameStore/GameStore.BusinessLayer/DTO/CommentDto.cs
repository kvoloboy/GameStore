using System.Collections.Generic;

namespace GameStore.BusinessLayer.DTO
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string GameKey { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string QuoteText { get; set; }
        public string ParentId { get; set; }
        public CommentDto Parent { get; set; }
        public IEnumerable<CommentDto> Children { get; set; }
    }
}