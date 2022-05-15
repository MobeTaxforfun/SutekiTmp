using System;
using System.Collections.Generic;

namespace SutekiTmp.Models.Entity
{
    public partial class Role
    {
        public Role()
        {
            RoleMenuPermissions = new HashSet<RoleMenuPermission>();
            Menus = new HashSet<Menu>();
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<RoleMenuPermission> RoleMenuPermissions { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
