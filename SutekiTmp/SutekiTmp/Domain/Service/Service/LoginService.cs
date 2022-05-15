using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Domain.Service.IService;
using SutekiTmp.Models.Entity;
using SutekiTmp.Models.Temp;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Service.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository userRepository;

        public LoginService(IUserRepository UserRepository)
        {
            userRepository = UserRepository;
        }

        public User GetUserByUserNameAndPassWord(string UserName, string PassWord)
        {
            return userRepository.GetUserByUserNameAndPassWord(UserName, PassWord);
        }
    }
}
