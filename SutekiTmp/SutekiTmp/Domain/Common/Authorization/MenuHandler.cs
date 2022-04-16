using Microsoft.AspNetCore.Authorization;
using SutekiTmp.Domain.Common.Authorization.Requirement;

namespace SutekiTmp.Domain.Common.Authorization
{
    public class MenuHandler : AuthorizationHandler<MenuRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MenuRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

}
