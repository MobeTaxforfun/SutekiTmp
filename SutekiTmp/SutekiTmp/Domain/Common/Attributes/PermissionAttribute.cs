using Microsoft.AspNetCore.Authorization;

namespace SutekiTmp.Domain.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class PermissionAttribute : AuthorizeAttribute
    {
        public string PermissionName { get; set; } = string.Empty;
        public PermissionAttribute(string PermissionName)
        {
            if (string.IsNullOrEmpty(PermissionName))
                throw new ArgumentNullException("必須有一個操作");
            this.PermissionName = PermissionName;
            Policy = $"Premission";
        }
    }
}
