using Microsoft.EntityFrameworkCore;
using SutekiTmp.Domain.Repository.IRepository;
using SutekiTmp.Models.Entity;
using SutekiTmp.Models.Temp;
using SutekiTmp.Models.Temp.Data;
using SutekiTmp.Viewmodels.Login;

namespace SutekiTmp.Domain.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TempDataContext context;

        public UserRepository(TempDataContext context)
        {
            this.context = context;
        }


        public User? GetUserById(int Id)
        {
            return context.Users.Include(c => c.Roles).FirstOrDefault(c => c.UserId == Id);
        }

        public User? GetUserByUserNameAndPassWord(string UserName, string PassWord)
        {

            var result = context.Users.FirstOrDefault(c => c.UserName == UserName && c.PassWord == PassWord);
            if (result == null)
            {
                return result;
            }
            context.Entry(result).Collection(c => c.Roles).Load();
            return result;
        }

        public List<Role> ListedUserRolesByUserId(int Id)
        {
            var user = context.Users.Find(Id);
            if (user == null)
            {
                return null;
            }
            else
            {
                return user.Roles.ToList();
            }
        }
    }
}
