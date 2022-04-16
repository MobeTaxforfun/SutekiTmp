namespace SutekiTmp.Domain.Repository.IRepository
{
    public interface IRoleMenuPromisionRepository
    {
        int GetPromissionIdByRoleIdAndMeun(int RoleId, int MenuId);

        List<int> ListedPromissionIdByRoleIdAndMeun(int RoleId, int MenuId);
    }
}
