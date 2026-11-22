using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pikagram.Enums;
using Pikagram.Models;
using Pikagram.Services;
using Pikagram.Utils;
using Pikagram.ViewModels;

namespace Pikagram.Controllers;

public class PostsController(AppDbContext db, UserManager<User> userManager, MediaService mediaService) : Controller
{
    [Route("posts/{*postId:guid}")]
    public IActionResult PostPage(Guid postId)
    {
        var post = db.Posts.FirstOrDefault(x => x.Id == postId);
        ViewBag.Post = post;
        return View("PostPage");
    }
    
    public record CreateCommentRequest(string Content);
    
    [Authorize]
    [HttpPost("posts/{postId:guid}/comment")]
    public async Task<IActionResult> Comment(Guid postId, [FromForm] CreateCommentRequest request)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }
        
        var comment = new Comment(userId.Value, postId, request.Content, DateTime.UtcNow);
        db.Comments.Add(comment);
        
        await db.SaveChangesAsync();
        return LocalRedirect($"/posts/{postId}");
    }
    
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreatePostViewModel model)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Media == null)
        {
            ModelState.AddModelError(nameof(model.Media), "Soubor je prázdný.");
            return View(model);
        }

        var post = new Post(userId.Value, model.Text, model.Tags, "", MediaType.Image);
        var saveResult = await mediaService.SaveMedia(model.Media, "posts", post.Id.ToString());
        if (!saveResult.HasValue)
        {
            return BadRequest("Soubor není ani obrázek, ani video");
        }
        
        post.ChangeSrc(saveResult.Value.relpath, saveResult.Value.type);
            
        db.Posts.Add(post);
        await db.SaveChangesAsync();
        return LocalRedirect($"/posts/{post.Id}");
    }
}