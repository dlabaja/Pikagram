using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pikagram.Enums;
using Pikagram.Models;
using Pikagram.Views.Shared;

namespace Pikagram.Controllers;

public class HomeController(AppDbContext db, UserManager<User> userManager) : Controller
{
    public async Task<IActionResult> Index(int tab = 1)
    {
        tab = Math.Clamp(tab, 1, 4);
        ViewBag.Posts = new List<Post>();
        ViewBag.Likes = new List<Like>();
        
        var user = await userManager.GetUserAsync(User);
        ViewBag.Likes = user?.Likes ?? new List<Like>();
        
        if (tab == 1 || user == null)
        {
            ViewBag.Posts = await db.Posts
                .OrderByDescending(x => x.DatePublished)
                .Take(50)
                .ToListAsync();
            return View();
        }
        
        switch (tab)
        {
            case 2:
                ViewBag.Posts = await db.Posts
                    .Where(x => x.AuthorId == user.Id)
                    .OrderByDescending(x => x.DatePublished)
                    .ToListAsync();
                break;
            case 3:
                var likedIds = await db.Likes
                    .Where(x => x.UserId == user.Id && x.LikedType == LikedType.Post)
                    .Select(x => x.LikedId)
                    .ToListAsync();
                ViewBag.Posts = user.Posts
                    .Where(x => likedIds.Contains(x.Id))
                    .ToList();
                break;
            case 4:
                ViewBag.Posts = user.Following
                    .SelectMany(x => x.FollowingUser.Posts)
                    .ToList();
                break;
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
