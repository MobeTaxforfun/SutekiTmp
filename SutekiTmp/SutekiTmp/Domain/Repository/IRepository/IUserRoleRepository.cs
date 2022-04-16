namespace SutekiTmp.Domain.Repository.IRepository
{
    public interface IUserRoleRepository
    {
        int GetRoleIdByUserId(int UserId);
    }
}
