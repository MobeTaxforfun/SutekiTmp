using Microsoft.EntityFrameworkCore;
using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Entity;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly TempDataContext tempDataContext;

        public RoleRepository(TempDataContext tempDataContext)
        {
            this.tempDataContext = tempDataContext;
        }
        public Role GetRoleById(int Id)
        {
            return tempDataContext.Roles.Include(c => c.RoleMenuPermissions).ThenInclude(c => c.Permission).FirstOrDefault(c => c.RoleId == Id);
        }
    }
}
