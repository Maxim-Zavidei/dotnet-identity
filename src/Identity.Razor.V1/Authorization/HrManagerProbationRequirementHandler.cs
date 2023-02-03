using Microsoft.AspNetCore.Authorization;

namespace Identity.Razor.V1.Authorization;

public class HrManagerProbationRequirementHandler : AuthorizationHandler<HrManagerProbationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HrManagerProbationRequirement requirement)
    {
        // The reason why we don't fail, but return Task.CompletedTask is because in some cases
        // we might want to have a few requirements and if it satisfies at least one it should pass.
        if (!context.User.HasClaim(e => e.Type == "EmploymentDate")) return Task.CompletedTask;

        var employmentDate = DateTime.Parse(context.User.FindFirst(e => e.Type == "EmploymentDate")!.Value);
        var period = DateTime.Now - employmentDate;
        if (period.Days > 30 * requirement.probationMonths) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
