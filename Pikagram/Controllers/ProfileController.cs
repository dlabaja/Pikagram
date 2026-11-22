using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Enums;
using Pikagram.Models;

namespace Pikagram.Controllers;

[Route("profile")]
public class ProfileController(AppDbContext db, UserManager<User> userManager) : Controller
{
    [HttpGet("{*username}")]
    public async Task<IActionResult> ProfilePage(string? username, int tab)
    {
        if (username == null)
        {
            return LocalRedirect("/");
        }
        
        var user = db.Users
            .Include(user => user.Posts)
            .Include(user => user.Likes)
            .Include(user => user.Comments)
            .FirstOrDefault(x => x.UserName == username.ToLower());
        if (user == null)
        {
            return NotFound();
        }
        
        tab = Math.Clamp(tab, 1, 4);

        List<Guid> likedIds = [];
        switch (tab)
        {
            case 1:
                ViewBag.Posts = user.Posts;
                break;
            case 2:
                likedIds = user.Likes
                    .Where(x => x.UserId == user.Id && x.LikedType == LikedType.Post)
                    .Select(x => x.LikedId)
                    .ToList();
                ViewBag.LikedPosts = user.Posts
                    .Where(x => likedIds.Contains(x.Id))
                    .ToList();
                break;
            case 3:
                ViewBag.Comments = user.Comments;
                break;
            case 4:
                likedIds = user.Likes
                    .Where(x => x.UserId == user.Id && x.LikedType == LikedType.Comment)
                    .Select(x => x.LikedId)
                    .ToList();
                ViewBag.LikedComments = user.Comments
                    .Where(x => likedIds.Contains(x.Id))
                    .ToList();
                break;
        }

        ViewBag.Likes = user.Likes;
        ViewBag.User = user;
        return View("ProfilePage");
    }
}