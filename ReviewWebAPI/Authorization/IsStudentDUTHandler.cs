using Microsoft.AspNetCore.Authorization;

namespace ReviewWebAPI.Authorization
{
    public class IsStudentDUTHandler : AuthorizationHandler<IsStudentDUTRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsStudentDUTRequirement requirement)
        {
            string schoolName = context.User.FindFirst("school").Value;
            if (string.IsNullOrEmpty(schoolName) || !schoolName.Contains(requirement.SchoolName))
            {
                var failure = new AuthorizationFailureReason(this, "You are not the DUT student");
                context.Fail(reason: failure);
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}