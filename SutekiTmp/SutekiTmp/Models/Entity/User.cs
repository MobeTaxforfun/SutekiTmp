using System;
using System.Collections.Generic;

namespace SutekiTmp.Models.Entity
{
    public partial class User
    {
        public User()
        {
            Roles = new HashSet<Role>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PassWord { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Role> Roles { get; set; }
    }
}
