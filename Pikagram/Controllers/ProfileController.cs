using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pikagram.Models;

namespace Pikagram.Controllers;

[Route("profile")]
public class ProfileController(AppDbContext db, UserManager<User> userManager) : Controller
{
    [HttpGet("{*username}")]
    public async Task<IActionResult> ProfilePage(string? username)
    {
        if (username == null)
        {
            return LocalRedirect("/");
        }

        var user = db.Users.FirstOrDefault(x => x.UserName == username.ToLower());
        if (user == null)
        {
            return NotFound();
        }
        
        ViewBag.User = user;
        return View("ProfilePage");
    }
}