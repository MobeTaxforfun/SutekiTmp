using Microsoft.AspNetCore.Authorization;

namespace SutekiTmp.Domain.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class MenuAttribute : Attribute
    {
        public string MenuName { get; set; } = string.Empty;
        public MenuAttribute(string MenuName)
        {
            if (string.IsNullOrEmpty(MenuName))
                throw new ArgumentNullException("必須有一個選單名稱");

            this.MenuName = MenuName;
        }
    }
}
