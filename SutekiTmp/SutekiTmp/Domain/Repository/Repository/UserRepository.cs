using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Temp;
using SutekiTmp.Models.Temp.Data;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class UserRepository : IUserRepository
    {      
        public UserRepository()
        {

        }

        public UserModel GetUser(UserModel userModel)
        {
            return TempData.users.FirstOrDefault(x => x.UserName.ToLower() == userModel.UserName.ToLower()
                                          && x.Password == userModel.Password);
        }
    }
}
