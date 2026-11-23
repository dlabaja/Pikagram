using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Models;
using Pikagram.Utils;

namespace Pikagram.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/users")]
public class ApiUserController(AppDbContext db, UserManager<User> userManager) : Controller
{
    [HttpPost("{requestedUserId:guid}/follow")]
    public async Task<IActionResult> Follow(Guid requestedUserId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        if (userId == requestedUserId)
        {
            return BadRequest("Nemůžeš sledovat sám sebe");
        }
        
        db.Followers.Add(new Follower(userId.Value, requestedUserId));
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("{requestedUserId:guid}/unfollow")]
    public async Task<IActionResult> Unfollow(Guid requestedUserId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }
        
        if (userId == requestedUserId)
        {
            return BadRequest("Nemůžeš sledovat sám sebe");
        }
        
        var rowsDeleted = await db.Followers
            .Where(x => x.FollowerId == userId.Value && x.FollowingId == requestedUserId)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            return NotFound("Uživatelé se nesledují");
        }
        
        return Ok();
    }
}