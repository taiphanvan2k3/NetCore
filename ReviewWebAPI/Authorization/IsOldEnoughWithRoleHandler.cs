using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ReviewWebAPI.Authorization
{
    public class IsOldEnoughWithRoleHandler : AuthorizationHandler<IsOldEnoughWithRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOldEnoughWithRoleRequirement requirement)
        {
            string birthDay = context.User.FindFirst(claim => claim.Type == ClaimTypes.DateOfBirth).Value;
            var birthDayObj = Convert.ToDateTime(birthDay);
            var now = DateTime.Today;
            int age = now.Year - birthDayObj.Year;
            if (now < birthDayObj.AddYears(age)) age--;

            if (requirement.MinAge <= age && context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}