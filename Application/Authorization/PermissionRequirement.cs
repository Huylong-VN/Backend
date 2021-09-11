using Backend.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Application.Authorization
{
    public class CustomClaimTypes
    {
        public const string Permission = "permission";
    }

    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // There can only be one policy provider in ASP.NET Core.
            // We only handle permissions related policies, for the rest
            /// we will use the default provider.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        // Dynamically creates a policy with a requirement that contains the permission.
        // The policy name must match the permission that is needed.
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permissions", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }

            // Policy is not for permissions, try the default provider.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }
    }

    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;

        public PermissionAuthorizationHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Get all the roles the user belongs to and check if any of the roles has the permission required
            // for the authorization to succeed.
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null) throw new UnauthorizedAccessException("You must Login");
            var userClaims = await _userManager.GetClaimsAsync(user);
            var permissions = userClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                              x.Value == requirement.Permission &&
                                              x.Issuer == "LOCAL AUTHORITY").Select(x => x.Value);
            if (permissions.Any())
            {
                context.Succeed(requirement);
                return;
            }
            context.Fail();

            //var userRoleNames = await _userManager.GetRolesAsync(user);
            //var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name));

            //foreach (var role in userRoles)
            //{
            //    var roleClaims = await _roleManager.GetClaimsAsync(role);
            //    var permissions = roleClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
            //                                            x.Value == requirement.Permission &&
            //                                            x.Issuer == "LOCAL AUTHORITY")
            //                                .Select(x => x.Value);

            //    if (permissions.Any())
            //    {
            //        context.Succeed(requirement);
            //        return;
            //    }
            //    context.Fail();
            //}
        }
    }
}