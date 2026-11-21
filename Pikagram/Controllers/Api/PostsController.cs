using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Enums;
using Pikagram.Models;
using Pikagram.Services;
using Pikagram.Utils;

namespace Pikagram.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/posts")]
public class PostsController(AppDbContext db, UserManager<User> userManager, MediaService mediaService) : Controller
{
    public record CreatePostRequest(string Text, IFormFile Media);
    public record EditPostRequest(string? Text, string? Tags);
    public record CreateCommentRequest(string Content);
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var post = new Post(userId.Value, request.Text, "", MediaType.Image);
        var saveResult = await mediaService.SaveMedia(request.Media, "posts", post.Id.ToString());
        if (!saveResult.HasValue)
        {
            return BadRequest("Soubor není ani obrázek, ani video");
        }
        
        post.ChangeSrc(saveResult.Value.relpath, saveResult.Value.type);
            
        db.Posts.Add(post);
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("{postId:guid}/comment")]
    public async Task<IActionResult> Comment(Guid postId, [FromBody] CreateCommentRequest request)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }
        
        var comment = new Comment(userId.Value, request.Content, DateTime.UtcNow, null);
        db.Comments.Add(comment);
        
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet("{postId:guid}/like")]
    public async Task<IActionResult> Like(Guid postId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        db.Likes.Add(new Like(userId.Value, postId, LikedType.Post));

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("Na příspěvku už like je");
        }

        return Ok();
    }
    
    [HttpGet("{postId:guid}/unlike")]
    public async Task<IActionResult> Unlike(Guid postId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var rowsDeleted = await db.Likes
            .Where(x => x.LikedId == postId && x.UserId == userId && x.LikedType == LikedType.Comment)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            return NotFound("Příspěvek žádný tvůj like nemá");
        }

        return Ok();
    }
    
    [HttpGet("{postId:guid}/edit")]
    public async Task<IActionResult> EditText(Guid postId, EditPostRequest request)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var post = db.Posts.FirstOrDefault(x => x.Id == postId);
        if (post == null)
        {
            return NotFound("Příspěvek neexistuje");
        }

        post.ChangeText(request.Text ?? post.Text);
        post.ChangeTags(request.Tags ?? post.Tags);
        await db.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("{postId:guid}/delete")]
    public async Task<IActionResult> Delete(Guid postId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var rowsDeleted = await db.Posts
            .Where(x => x.Id == postId)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            return NotFound("Příspěvek nenalezen");
        }

        return Ok();
    }
}