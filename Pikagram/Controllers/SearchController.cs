using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pikagram.Models;
using Pikagram.ViewModels;

namespace Pikagram.Controllers;

public class SearchController(AppDbContext db, UserManager<User> userManager) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new SearchViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(SearchViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var users = db.Users
            .Where(x => x.UserName != null && x.UserName.StartsWith(model.Text.Trim().ToLower()))
            .ToList();

        ViewBag.Users = users;
        ViewBag.Following = new List<Guid>();
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return View(model);
        }

        ViewBag.Following = user.Following.Select(x => x.FollowingId).ToList();
        return View(model);
    }
}