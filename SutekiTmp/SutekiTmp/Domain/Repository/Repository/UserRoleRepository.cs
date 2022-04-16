using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Temp.Data;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        public UserRoleRepository()
        {

        }

        public int GetRoleIdByUserId(int UserId)
        {
            return TempData.userRoles.FirstOrDefault(x => x.UserId == UserId).RuleId;
        }
    }
}
