using Microsoft.AspNetCore.Mvc;
using Pikogram.Models;
using Pikogram.Models.Comments;

namespace Pikogram.Controllers.Api;

[ApiController]
[Route("api/posts")]
public class PostsController(AppDbContext db) : Controller
{
    public record CreateCommentRequest(string Content);
    private User mockUser = new User("dlabaja");
    
    [HttpPost("{postId:guid}/comment")]
    public async Task<IActionResult> Comment(Guid postId, [FromBody] CreateCommentRequest request)
    {
        var post = await db.Posts.FindAsync(postId);
        if (post == null)
        {
            return NotFound();
        }
        
        var comment = new Comment(mockUser, request.Content, DateTime.UtcNow, null);
        post.Comments.Add(comment);
        
        db.Comments.Add(comment);
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet("{}")]
    public async Task<IActionResult> GetComments()
    {
        /*var comment = await db.Comments.FindAsync(commentId);
        if (comment == null)
        {
            return NotFound();
        }
        
        var reply = new Comment(mockUser, request.Content, DateTime.UtcNow, comment);
        db.Comments.Add(reply);
        await db.SaveChangesAsync();*/
        return Ok();
    }
}