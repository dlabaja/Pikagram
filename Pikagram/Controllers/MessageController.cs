using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Models;
using Pikagram.Utils;
using Pikagram.ViewModels;

namespace Pikagram.Controllers;

[Authorize]
[Route("message")]
public class MessageController(AppDbContext db, UserManager<User> userManager) : Controller
{
    [HttpGet("{receiverId:guid}")]
    public async Task<IActionResult> MessagePage(Guid receiverId)
    {
        var senderId = userManager.GetUserGuid(User);
        if (!senderId.HasValue)
        {
            return Unauthorized();
        }

        var messages = await db.Messages
            .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .Where(x =>
                (x.SenderId == senderId && x.ReceiverId == receiverId) ||
                (x.SenderId == receiverId && x.ReceiverId == senderId))
            .OrderByDescending(x => x.DateSent)
            .ToListAsync();

        ViewBag.ReceiverId = receiverId;
        ViewBag.Messages = messages;
        return View("MessagePage");
    }
    
    [HttpPost("{receiverId:guid}/send")]
    public async Task<IActionResult> Send(Guid receiverId, SendMessageViewModel model)
    {
        var senderId = userManager.GetUserGuid(User);
        if (!senderId.HasValue)
        {
            return Unauthorized();
        }

        var message = new Message(senderId.Value, receiverId, model.Message);
        db.Messages.Add(message);
        await db.SaveChangesAsync();
        return LocalRedirect(GetMessageUrl(receiverId));
    }

    [HttpPost("{receiverId:guid}/delete/{messageId:guid}")]
    public async Task<IActionResult> Delete(Guid receiverId, Guid messageId)
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

        return LocalRedirect(GetMessageUrl(receiverId));
    }
    
    private string GetMessageUrl(Guid receiverId)
    {
        return $"/message/{receiverId}";
    }
}