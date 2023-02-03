using Microsoft.AspNetCore.Authorization;

namespace Identity.Razor.V1.Authorization;

public class HrManagerProbationRequirement : IAuthorizationRequirement
{
    public int probationMonths { get; set; }

    public HrManagerProbationRequirement(int probationMonths)
    {
        this.probationMonths = probationMonths;
    }
}
