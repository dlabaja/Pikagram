using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Enums;
using Pikagram.Models;
using Pikagram.Utils;

namespace Pikagram.Controllers;

[Route("profile")]
public class ProfileController(AppDbContext db, UserManager<User> userManager) : Controller
{
    [HttpGet("{username}")]
    public IActionResult ProfilePage(string? username, int tab = 1)
    {
        if (username == null)
        {
            return LocalRedirect("/");
        }
        
        var user = db.Users
            .Include(user => user.Posts)
            .Include(user => user.Likes)
            .Include(user => user.Comments)
            .Include(user => user.Followers)
            .Include(user => user.Following)
            .FirstOrDefault(x => x.UserName == username.ToLower());
        if (user == null)
        {
            return NotFound();
        }

        var userId = userManager.GetUserGuid(User);
        ViewBag.FollowerCount = user.Followers.Count;
        ViewBag.FollowingCount = user.Following.Count;
        
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

        ViewBag.IsFollowing = user.Followers.Select(x => x.FollowerId).Any(x => x == userId);
        ViewBag.Likes = user.Likes;
        ViewBag.User = user;
        return View("ProfilePage");
    }

    [HttpGet("{username}/followers")]
    public async Task<IActionResult> Followers(string username)
    {
        ViewBag.UserFollowing = new List<Guid>();
        ViewBag.Followers = db.Users
            .Include(x => x.Following)
            .Where(x => x.UserName == username)
            .SelectMany(x => x.Followers)
            .Select(x => x.FollowerUser)
            .ToList();
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return View("Followers");
        }
        
        ViewBag.Username = username;
        ViewBag.UserFollowing = user.Following.Select(x => x.FollowingId).ToList(); 
        return View("Followers");
    }
    
    [HttpGet("{username}/following")]
    public async Task<IActionResult> Following(string username)
    {
        ViewBag.UserFollowing = new List<Guid>();
        ViewBag.Following = db.Users
            .Include(x => x.Following)
            .Where(x => x.UserName == username)
            .SelectMany(x => x.Following)
            .Select(x => x.FollowingUser)
            .ToList();
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return View("Following");
        }

        ViewBag.Username = username;
        ViewBag.UserFollowing = user.Following.Select(x => x.FollowingId).ToList(); 
        return View("Following");
    }
}