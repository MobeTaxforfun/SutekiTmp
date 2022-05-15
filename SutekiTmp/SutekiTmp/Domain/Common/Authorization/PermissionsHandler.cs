using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using SutekiTmp.Domain.Common.Attributes;
using SutekiTmp.Domain.Common.Authentication.Session;
using SutekiTmp.Domain.Common.Authorization.Requirement;
using SutekiTmp.Domain.Repository.IRepository;
using System.Security.Claims;

namespace SutekiTmp.Domain.Common.Authorization
{
    public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMeunRepository meunRepository;
        private readonly IRoleRepository roleRepository;

        public PermissionsHandler(
            IHttpContextAccessor httpContextAccessor,
            IMeunRepository meunRepository,
            IRoleRepository roleRepository)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.meunRepository = meunRepository;
            this.roleRepository = roleRepository;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
        {

            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail(new AuthorizationFailureReason(this, "尚未登入"));
                return Task.CompletedTask;
            }

            var endpoint = httpContextAccessor.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            var MenuAttributre = endpoint?.Metadata.GetMetadata<MenuAttribute>();
            var PermissinAttribute = endpoint?.Metadata.GetMetadata<PermissionAttribute>();

            if (MenuAttributre == null)
                throw new ArgumentNullException(nameof(MenuAttribute));
            if (PermissinAttribute == null)
                throw new ArgumentNullException(nameof(PermissionAttribute));

            //檢查這個Role 有沒有瀏覽這個 Menu 的權限
            var RoleId = Int32.Parse(context.User.Claims.First(c => c.Type == ClaimTypes.Role).Value);
            var Menu = meunRepository.GetMenuByMenuName(MenuAttributre.MenuName);

            //加入此頁權限
            var Role = roleRepository.GetRoleById(RoleId);
            var Permissions = Role.Menus.First(c => c.MenuId == Menu.MenuId).RoleMenuPermissions.Select(c => c.Permission.PermissionName);         
            List<Claim> claims = new List<Claim>()
                    {
                    new Claim("Permissions",JsonConvert.SerializeObject(Permissions)),
                    };
            context.User.AddIdentity(new(claims, SessionAuthenticationOptions.Scheme));

            if (Menu.Roles.FirstOrDefault(c => c.RoleId == RoleId) == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "您無權限訪問此頁面"));
            }


            if (Menu.RoleMenuPermissions.FirstOrDefault(c => c.Permission.PermissionName == PermissinAttribute.PermissionName) == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "您的權限不足以訪問此頁面"));
            }
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
