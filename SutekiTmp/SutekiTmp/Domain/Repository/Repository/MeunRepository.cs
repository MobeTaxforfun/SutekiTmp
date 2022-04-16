using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Temp.Data;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class MeunRepository : IMeunRepository
    {
        public int GetMenuIdByMenuName(string menuName)
        {
            return TempData.meuns.First(c=>c.MenuName == menuName).MenuId;
        }
    }
}
