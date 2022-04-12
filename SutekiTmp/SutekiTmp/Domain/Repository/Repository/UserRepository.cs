using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Temp;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly List<UserModel> users = new List<UserModel>();

        public UserRepository()
        {
            users.Add(new UserModel
            {
                UserName = "mobewu",
                Password = "mobe000",
                Role = "Administrator"
            });
            users.Add(new UserModel
            {
                UserName = "paulwu",
                Password = "paulwu000",
                Role = "Admin"
            });
            users.Add(new UserModel
            {
                UserName = "zakowu",
                Password = "zakowu000",
                Role = "NormalUser"
            });
            users.Add(new UserModel
            {
                UserName = "roughfish",
                Password = "roughfish000",
                Role = "NormalUser"
            });
            users.Add(new UserModel
            {
                UserName = "rubberneck",
                Password = "rubberneck000",
                Role = "Visitor"
            });
        }

        public UserModel GetUser(UserModel userModel)
        {
            return users.FirstOrDefault(x => x.UserName.ToLower() == userModel.UserName.ToLower()
                                          && x.Password == userModel.Password);
        }
    }
}
