using Microsoft.AspNetCore.Mvc;
using Pikagram.Models;
using Pikagram.Models.Comments;

namespace Pikagram.Controllers.Api;

[ApiController]
[Route("api/comments")]
public class CommentsController(AppDbContext db) : Controller
{
    // record = datová třída, properties se vygenerují automaticky
    public record EditCommentRequest(string NewContent);
    public record ReplyCommentRequest(string Content);
    private User mockUser = new User("dlabaja");
    
    // route se dá i typovat, bengr
    [HttpPost("{commentId:guid}/edit")]
    public async Task<IActionResult> Edit(Guid commentId, [FromBody] EditCommentRequest request)
    {
        var comment = await db.Comments.FindAsync(commentId);
        if (comment == null)
        {
            return NotFound();
        }
        comment.Edit(request.NewContent);
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("{commentId:guid}/delete")]
    public async Task<IActionResult> Delete(Guid commentId)
    {
        var comment = await db.Comments.FindAsync(commentId);
        if (comment == null)
        {
            return NotFound();
        }

        db.Comments.Remove(comment);
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("{commentId:guid}/reply")]
    public async Task<IActionResult> Reply(Guid commentId, [FromBody] ReplyCommentRequest request)
    {
        var comment = await db.Comments.FindAsync(commentId);
        if (comment == null)
        {
            return NotFound();
        }
        
        var reply = new Comment(mockUser, request.Content, DateTime.UtcNow, comment);
        db.Comments.Add(reply);
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("{commentId:guid}/like")]
    public async Task<IActionResult> Like(Guid commentId)
    {
        /*var result = await db.Comments
            .Where(x => x.Id == commentId)
            .Select(x => new
            {
                AlreadyLiked = x.Likes.Any(y => y.UserId == mockUser.Id)
            })
            .FirstOrDefaultAsync();
        
        if (result == null)
        {
            return NotFound();
        }
        
        
        
        comment.Likes.Add();
        db.Comments.Add(reply);
        await db.SaveChangesAsync();*/
        return Ok();
    }
}