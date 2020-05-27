using System.Collections.Generic;

namespace GameStore.Core.Models.Identity
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}