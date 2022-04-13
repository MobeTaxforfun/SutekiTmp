using SutekiTmp.Models.Temp;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Service.IService
{
    public interface ILoginService
    {
        public UserModel GetUser(LoginViewModel loginViewModel);
    }
}
