using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SutekiTmp.Models.Temp.Data
{
    public class TempData
    {
        public static List<MeunModel> meuns = new List<MeunModel>();
        public static List<PromisionModel> promisions = new List<PromisionModel>();
        public static List<RoleMenuPromision> menuPromisions = new List<RoleMenuPromision>();
        public static List<RoleModel> roles = new List<RoleModel>();
        public static List<UserModel> users = new List<UserModel>();
        public static List<UserRoleModel> userRoles = new List<UserRoleModel>();

        static TempData()
        {
            meuns.Add(new MeunModel { MenuId = 1, MenuName = "帳號管理" });
            meuns.Add(new MeunModel { MenuId = 2, MenuName = "權限管理" });
            meuns.Add(new MeunModel { MenuId = 3, MenuName = "訂單管理" });

            promisions.Add(new PromisionModel { PromisionId = 1, PromisionCode = "View", PromisionName = "檢視" });
            promisions.Add(new PromisionModel { PromisionId = 2, PromisionCode = "Create", PromisionName = "新建" });
            promisions.Add(new PromisionModel { PromisionId = 3, PromisionCode = "Update", PromisionName = "更新" });
            promisions.Add(new PromisionModel { PromisionId = 4, PromisionCode = "Delete", PromisionName = "刪除" });

            menuPromisions.Add(new RoleMenuPromision { RoleId = 1, PromisionId = 1 });
            menuPromisions.Add(new RoleMenuPromision { RoleId = 1, PromisionId = 2 });
            menuPromisions.Add(new RoleMenuPromision { RoleId = 1, PromisionId = 3 });
            menuPromisions.Add(new RoleMenuPromision { RoleId = 1, PromisionId = 4 });
            menuPromisions.Add(new RoleMenuPromision { RoleId = 2, PromisionId = 1 });
            menuPromisions.Add(new RoleMenuPromision { RoleId = 2, PromisionId = 3 });
            menuPromisions.Add(new RoleMenuPromision { RoleId = 3, PromisionId = 1 });

            userRoles.Add(new UserRoleModel { RuleId = 1, UserId = 1 });
            userRoles.Add(new UserRoleModel { RuleId = 2, UserId = 2 });
            userRoles.Add(new UserRoleModel { RuleId = 3, UserId = 3 });
            userRoles.Add(new UserRoleModel { RuleId = 3, UserId = 3 });

            roles.Add(new RoleModel { RuleId = 1, RoleName = "Administrator" });
            roles.Add(new RoleModel { RuleId = 2, RoleName = "Admin" });
            roles.Add(new RoleModel { RuleId = 3, RoleName = "Visitor" });

            #region User
            users.Add(new UserModel
            {
                UserId = 1,
                UserName = "mobewu",
                Password = "mobe000",
                Email = "mobe@mail.com",
                Phone = "0900000001",
                IsEnable = true
            });
            users.Add(new UserModel
            {
                UserId = 2,
                UserName = "paulwu",
                Password = "paulwu000",
                Email = "paulwu@mail.com",
                Phone = "0900000002",
                IsEnable = true
            });
            users.Add(new UserModel
            {
                UserId = 3,
                UserName = "zakowu",
                Password = "zakowu000",
                Email = "zakowu@mail.com",
                Phone = "0900000003",
                IsEnable = true
            });
            users.Add(new UserModel
            {
                UserId = 4,
                UserName = "rubberneck",
                Password = "rubberneck000",
                Email = "rubberneck@mail.com",
                Phone = "0900000004",
                IsEnable = true
            });
            #endregion

        }
    }
}
