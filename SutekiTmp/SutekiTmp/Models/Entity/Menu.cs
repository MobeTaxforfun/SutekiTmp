using System;
using System.Collections.Generic;

namespace SutekiTmp.Models.Entity
{
    public partial class Menu
    {
        public Menu()
        {
            RoleMenuPermissions = new HashSet<RoleMenuPermission>();
            Roles = new HashSet<Role>();
        }

        public int MenuId { get; set; }
        public string MenuName { get; set; } = null!;

        public virtual ICollection<RoleMenuPermission> RoleMenuPermissions { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
