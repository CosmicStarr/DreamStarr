using System.Security.Claims;

namespace StarrAPI.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUsername(this ClaimsPrincipal User)
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}