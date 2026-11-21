using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Models;
using Pikagram.Utils;

namespace Pikagram.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/messages")]
public class ApiMessageController(AppDbContext db, UserManager<User> userManager) : Controller
{
    public record SendMessageRequest(Guid ReceiverId, string Text);

    
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var message = new Message(userId.Value, request.ReceiverId, request.Text);
        db.Messages.Add(message);
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{messageId:guid}/delete")]
    public async Task<IActionResult> Edit(Guid messageId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var rowsDeleted = await db.Messages
            .Where(x => x.Id == messageId && x.SenderId == userId)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            return NotFound();
        }

        return Ok();
    }
}