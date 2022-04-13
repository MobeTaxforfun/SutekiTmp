using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Domain.Service.IService;
using SutekiTmp.Models.Temp;
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
        public UserModel GetUser(LoginViewModel loginViewModel)
        {
            var result = _UserRepository.GetUser(new Models.Temp.UserModel { UserName = loginViewModel.UserName, Password = loginViewModel.Password });
            if (result == null)
                return null;
            return result;
        }

    }
}
