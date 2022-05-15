using SutekiTmp.Models.Entity;

namespace SutekiTmp.Domain.Repository.IRepository
{
    public interface IMeunRepository
    {
        public int GetMenuIdByMenuName(string menuName);
        public Menu GetMenuByMenuName(string MenuName);
    }
}
