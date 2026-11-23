using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Enums;
using Pikagram.Models;
using Pikagram.Utils;

namespace Pikagram.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/comments")]
public class ApiCommentsController(AppDbContext db, UserManager<User> userManager) : Controller
{
    [HttpPost("{commentId:guid}/delete")]
    public async Task<IActionResult> Delete(Guid commentId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }
        
        var rowsDeleted = await db.Comments
            .Where(x => x.Id == commentId && x.AuthorId == userId)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            return NotFound("Komentář žádný tvůj like nemá");
        }

        return Ok();
    }

    [HttpPost("{commentId:guid}/like")]
    public async Task<IActionResult> Like(Guid commentId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        db.Likes.Add(new Like(userId.Value, commentId, LikedType.Comment));

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("Na komentáři už like je");
        }

        return Ok();
    }

    [HttpPost("{commentId:guid}/unlike")]
    public async Task<IActionResult> Unlike(Guid commentId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var rowsDeleted = await db.Likes
            .Where(x => x.LikedId == commentId && x.UserId == userId && x.LikedType == LikedType.Comment)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            return NotFound("Komentář žádný tvůj like nemá");
        }

        return Ok();
    }
}