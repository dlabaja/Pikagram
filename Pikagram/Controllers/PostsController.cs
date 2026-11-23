using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Enums;
using Pikagram.Models;
using Pikagram.Services;
using Pikagram.Utils;
using Pikagram.ViewModels;

namespace Pikagram.Controllers;

[Route("posts")]
public class PostsController(AppDbContext db, UserManager<User> userManager, MediaService mediaService) : Controller
{
    public record CreateCommentRequest(string Content);
    
    [Route("{postId:guid}")]
    public IActionResult PostPage(Guid postId)
    {
        var userId = userManager.GetUserGuid(User);
        var post = db.Posts.FirstOrDefault(x => x.Id == postId);

        if (post == null)
        {
            return NotFound();
        }
        
        var likes = userId.HasValue
            ? db.Likes.Where(x => x.UserId == userId).ToList()
            : [];
        ViewBag.Post = post;
        ViewBag.Likes = likes;
        return View("PostPage");
    }
    
    [Authorize]
    [HttpPost("{postId:guid}/comment")]
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

        var post = new Post(userId.Value, model.Text, model.Tags ?? "", "", MediaType.Image);
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
    
    [Authorize]
    [HttpGet("{postId:guid}/edit")]
    public IActionResult Edit(Guid postId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }
        
        var post = db.Posts.FirstOrDefault(x => x.Id == postId);
        if (post == null)
        {
            return NotFound();
        }

        if (post.AuthorId != userId)
        {
            return Forbid();
        }

        var model = new EditPostViewModel()
        {
            Text = post.Text,
            Tags = post.Tags
        };

        ViewBag.Post = post;
        return View(model);
    }

    
    [Authorize]
    [HttpPost("{postId:guid}/edit")]
    public async Task<IActionResult> Edit(Guid postId, [FromForm] EditPostViewModel model)
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
        
        var post = db.Posts.FirstOrDefault(x => x.Id == postId);
        if (post == null)
        {
            return NotFound();
        }
        
        if (post.AuthorId != userId)
        {
            return Forbid();
        }
        
        post.ChangeText(model.Text ?? post.Text);
        post.ChangeTags(model.Tags ?? post.Tags);

        if (model.Media != null)
        {
            var saveResult = await mediaService.SaveMedia(model.Media, "posts", post.Id.ToString());
            if (!saveResult.HasValue)
            {
                return BadRequest("Soubor není ani obrázek, ani video");
            }

            // kvůli invalidaci cache v browseru, aby tam nezůstal původní obrázek
            var relpath = $"{saveResult.Value.relpath}?x={DateTime.UtcNow.Ticks}";
            post.ChangeSrc(relpath, saveResult.Value.type);
        }

        await db.SaveChangesAsync();
        return LocalRedirect($"/posts/{post.Id}");
    }
    
    [Authorize]
    [HttpPost("{postId:guid}/delete")]
    public async Task<IActionResult> Delete(Guid postId)
    {
        var userId = userManager.GetUserGuid(User);
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var rowsDeleted = await db.Posts
            .Where(x => x.Id == postId && x.AuthorId == userId)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            return NotFound("Příspěvek nenalezen");
        }

        return LocalRedirect("/");
    }
}