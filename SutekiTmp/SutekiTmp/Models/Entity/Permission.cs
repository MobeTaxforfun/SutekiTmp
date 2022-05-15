using System;
using System.Collections.Generic;

namespace SutekiTmp.Models.Entity
{
    public partial class Permission
    {
        public Permission()
        {
            RoleMenuPermissions = new HashSet<RoleMenuPermission>();
        }

        public int PermissionId { get; set; }
        public string PermissionCode { get; set; } = null!;
        public string PermissionName { get; set; } = null!;

        public virtual ICollection<RoleMenuPermission> RoleMenuPermissions { get; set; }
    }
}
