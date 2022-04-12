using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Domain.Service.IService;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Service.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _UserRepository;
        public LoginService(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }
        public LoginViewModel GetUser(LoginViewModel loginViewModel)
        {
            var result = _UserRepository.GetUser(new Models.Temp.UserModel { UserName = loginViewModel.UserName, Password = loginViewModel.Password });
            if (result == null)
                return null;
            return new LoginViewModel { UserName = result.UserName, Password = result.Password };
        }
    }
}
