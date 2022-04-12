using SutekiTmp.Models.Temp;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Repository.IRepository
{
    public interface IUserRepository
    {
        UserModel GetUser(UserModel loginViewModel);
    }
}
