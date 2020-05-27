using System.Collections.Generic;

namespace GameStore.BusinessLayer.DTO
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}