using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using SutekiTmp.Domain.Common.Attributes;
using SutekiTmp.Domain.Common.Authorization.Requirement;
using SutekiTmp.Domain.Repository.IRepository;

namespace SutekiTmp.Domain.Common.Authorization
{
    public class MenuPerssionHanlder : AuthorizationHandler<MenuRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleMenuPromisionRepository _roleMenuProisionRepository;
        private readonly IMeunRepository _meunRepository;
        public MenuPerssionHanlder(
            IHttpContextAccessor httpContextAccessor,
            IRoleMenuPromisionRepository roleMenuPromisionRepository,
            IMeunRepository meunRepository)
        {
            _meunRepository = meunRepository;
            _roleMenuProisionRepository = roleMenuPromisionRepository;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MenuRequirement requirement)
        {
            var endpoint = _httpContextAccessor.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<MenuAttribute>();

            if (attribute == null)
                throw new ArgumentNullException(nameof(MenuAttribute));

            var UserId = Int32.Parse(context.User.Claims.First(c => c.Type == "UserId").Value);
            var RoleId = Int32.Parse(context.User.Claims.First(c => c.Type == "RoleId").Value);

         
            var MenuId = _meunRepository.GetMenuIdByMenuName(attribute.MenuName);

            var PromisionId = _roleMenuProisionRepository.GetPromissionIdByRoleIdAndMeun(UserId, MenuId);

            if (PromisionId == 0)
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
