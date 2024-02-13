using Microsoft.AspNetCore.Authorization;

namespace ReviewWebAPI.Authorization
{
    public class IsStudentDUTRequirement : IAuthorizationRequirement
    {
        public IsStudentDUTRequirement(string schoolName)
        {
            SchoolName = schoolName;
        }
        
        public string SchoolName { get; set; }
    }
}