using System.Security.Claims;

namespace Files.Services.Infrastructure;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
}