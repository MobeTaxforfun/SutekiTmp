using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Service.IService
{
    public interface ILoginService
    {
        public LoginViewModel GetUser(LoginViewModel loginViewModel);
    }
}
