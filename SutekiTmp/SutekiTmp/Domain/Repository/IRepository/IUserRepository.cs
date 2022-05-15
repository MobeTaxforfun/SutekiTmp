using SutekiTmp.Models.Entity;
using SutekiTmp.Models.Temp;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Repository.IRepository
{
    public interface IUserRepository
    {
        public User? GetUserByUserNameAndPassWord(string UserName, string PassWord);

        public User? GetUserById(int Id);

        public List<Role> ListedUserRolesByUserId(int Id);
    }
}
