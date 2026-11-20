using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using static System.Guid;

namespace Pikagram.Utils;

public static class UserManagerUtils
{
    public static Guid? GetUserGuid<T>(this UserManager<T> userManager, ClaimsPrincipal principal) where T : IdentityUser<Guid>
    {
        TryParse(userManager.GetUserId(principal), out var res);
        return res;
    }
}