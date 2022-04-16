using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Temp.Data;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class RoleMenuPromisionRepository : IRoleMenuPromisionRepository
    {
        public int GetPromissionIdByRoleIdAndMeun(int RoleId, int MenuId)
        {
           return TempData.menuPromisions.First(c=>c.RoleId == RoleId && c.MenuId == MenuId).PromisionId;
        }

        public List<int> ListedPromissionIdByRoleIdAndMeun(int RoleId, int MenuId)
        {
            return TempData.menuPromisions.Where(c => c.RoleId == RoleId && c.MenuId == MenuId).Select(c=>c.PromisionId).ToList();
        }
    }
}
