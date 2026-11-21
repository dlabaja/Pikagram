using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pikagram.Enums;
using Pikagram.Models;
using Pikagram.Services;

namespace Pikagram.Controllers.Api;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController(AppDbContext db, UserManager<User> userManager, MediaService mediaService) : Controller
{
    public record ChangeMediaRequest(IFormFile Media);
    public record EditProfileRequest(string? Bio);
    
    [HttpPut("change-avatar")]
    public async Task<IActionResult> ChangeAvatar([FromBody] ChangeMediaRequest request)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        var saveResult = await mediaService.SaveMedia(request.Media, "avatars", user.Id.ToString());
        if (!saveResult.HasValue)
        {
            return BadRequest("Neplatná profilovka");
        }

        if (saveResult.Value.type != MediaType.Image)
        {
            mediaService.RemoveMedia(saveResult.Value.relpath);
            return BadRequest("Neplatný formát obrázku");
        }
        
        user.ChangeAvatarSrc(saveResult.Value.relpath);
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPut("change-background")]
    public async Task<IActionResult> ChangeBackground([FromBody] ChangeMediaRequest request)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        var saveResult = await mediaService.SaveMedia(request.Media, "backgrounds", user.Id.ToString());
        if (!saveResult.HasValue)
        {
            return BadRequest("Neplatné pozadí");
        }

        if (saveResult.Value.type != MediaType.Image)
        {
            mediaService.RemoveMedia(saveResult.Value.relpath);
            return BadRequest("Neplatný formát obrázku");
        }
        
        user.ChangeBackgroundSrc(saveResult.Value.relpath);
        await db.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("edit")]
    public async Task<IActionResult> Edit([FromBody] EditProfileRequest request)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        user.ChangeBio(request.Bio ?? user.Bio);
        await db.SaveChangesAsync();
        return Ok();
    }
}