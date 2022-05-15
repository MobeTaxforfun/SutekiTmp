using Microsoft.EntityFrameworkCore;
using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Entity;
using SutekiTmp.Models.Temp.Data;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class MeunRepository : IMeunRepository
    {
        private readonly TempDataContext tempDataContext;

        public MeunRepository(TempDataContext tempDataContext)
        {
            this.tempDataContext = tempDataContext;
        }
        public int GetMenuIdByMenuName(string menuName)
        {
            return tempDataContext.Menus.FirstOrDefault(c => c.MenuName == menuName).MenuId;
        }

        public Menu GetMenuByMenuName(string MenuName)
        {
            return tempDataContext.Menus
                .Include(c => c.Roles)
                .Include(c => c.RoleMenuPermissions).ThenInclude(r => r.Permission)
                .FirstOrDefault(c => c.MenuName == MenuName);
        }
    }
}
