using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace EquipmentRentalUI.Services
{
    public class RoleClaimsTransformer : IClaimsTransformation
    {
        private readonly IConfiguration _config;

        public RoleClaimsTransformer(IConfiguration config)
        {
            _config = config;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
                return Task.FromResult(principal);

            var email = identity.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Task.FromResult(principal);

            // Remove existing role claims
            foreach (var oldRole in identity.FindAll(ClaimTypes.Role).ToList())
                identity.RemoveClaim(oldRole);

            // Get admin list from appsettings.json
            var adminEmails = _config.GetSection("AuthDemo:AdminEmails").Get<string[]>() ?? Array.Empty<string>();

            // Assign role based on email
            var role = adminEmails.Contains(email, StringComparer.OrdinalIgnoreCase) ? "Admin" : "User";
            identity.AddClaim(new Claim(ClaimTypes.Role, role));

            // Debug output
            Console.WriteLine($"🟢 Role assigned to {email}: {role}");

            return Task.FromResult(principal);
        }
    }
}
