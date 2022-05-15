using SutekiTmp.Models.Entity;
using SutekiTmp.Models.Temp;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Service.IService
{
    public interface ILoginService
    {
        public User GetUserByUserNameAndPassWord(string UserName, string PassWord);
    }
}
