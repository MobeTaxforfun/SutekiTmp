using SutekiTmp.Models.Entity;

namespace SutekiTmp.Domain.Repository.IRepository
{
    public interface IRoleRepository
    {
        public Role GetRoleById(int Id);
    }
}
