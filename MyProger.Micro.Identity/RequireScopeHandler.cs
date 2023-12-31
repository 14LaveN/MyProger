using Microsoft.AspNetCore.Authorization;

namespace MyProger.Micro.Identity;

public class RequireScopeHandler : AuthorizationHandler<ScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
    {
        var scopeClaim = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer);

        if (scopeClaim == null || string.IsNullOrWhiteSpace(scopeClaim.Value))
        {
            context.Fail(new AuthorizationFailureReason(this, "Scopes was null"));
            return Task.CompletedTask;
        }

        if (scopeClaim.Value.Split(' ').Any(s => s == requirement.Scope))
            context.Succeed(requirement);
        else
            context.Fail(new AuthorizationFailureReason(this, "Scope invalid"));
        
        return Task.CompletedTask;
    }
}

public class ScopeRequirement : IAuthorizationRequirement
{
    public string Issuer { get; }
    public string Scope { get; }

    public ScopeRequirement(string issuer, string scope)
    {
        Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));
    }
}