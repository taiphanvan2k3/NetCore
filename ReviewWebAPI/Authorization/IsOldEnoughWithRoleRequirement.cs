using Microsoft.AspNetCore.Authorization;

namespace ReviewWebAPI.Authorization
{
    public class IsOldEnoughWithRoleRequirement : IAuthorizationRequirement
    {
        public int MinAge { get; set; }

        public IsOldEnoughWithRoleRequirement(int minAge)
        {
            MinAge = minAge;
        }
    }
}