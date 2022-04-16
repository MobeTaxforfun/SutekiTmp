using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using SutekiTmp.Domain.Common.Attributes;
using SutekiTmp.Domain.Common.Authorization.Requirement;
using SutekiTmp.Domain.Repository.IRepository;

namespace SutekiTmp.Domain.Common.Authorization
{
    public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
    {
        private readonly IPromisionRepository _promisionRepository;
        private readonly IRoleMenuPromisionRepository _roleMenuProisionRepository;
        private readonly IMeunRepository _meunRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionsHandler(
            IHttpContextAccessor httpContextAccessor,
            IPromisionRepository promisionRepository,
            IMeunRepository meunRepository,
            IRoleMenuPromisionRepository roleMenuPromisionRepository)
        {
            _roleMenuProisionRepository = roleMenuPromisionRepository;
            _meunRepository = meunRepository;
            _promisionRepository = promisionRepository;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
        {
            var endpoint = _httpContextAccessor.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            var MenuAttributre = endpoint?.Metadata.GetMetadata<MenuAttribute>();
            var PermissinAttribute = endpoint?.Metadata.GetMetadata<PermissionAttribute>();

            if (MenuAttributre == null)
                throw new ArgumentNullException(nameof(MenuAttribute));
            if (PermissinAttribute == null)
                throw new ArgumentNullException(nameof(PermissionAttribute));

            var UserId = Int32.Parse(context.User.Claims.First(c => c.Type == "UserId").Value);
            var RoleId = Int32.Parse(context.User.Claims.First(c => c.Type == "RoleId").Value);
            var MenuId = _meunRepository.GetMenuIdByMenuName(MenuAttributre.MenuName);
            var ListOfPromissionId = _roleMenuProisionRepository.ListedPromissionIdByRoleIdAndMeun(UserId, MenuId);
            var PromissionId = _promisionRepository.GetPromisionIdByPromisionName(PermissinAttribute.PermissionName);

            if (ListOfPromissionId.Contains(PromissionId))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(new AuthorizationFailureReason(this, "您無權限訪問此頁面"));
            }

            return Task.CompletedTask;
        }
    }
}
