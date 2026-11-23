using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pikagram.Enums;
using Pikagram.Models;
using Pikagram.Services;
using Pikagram.ViewModels.ProfileViewModel;
using static Pikagram.Utils.UrlUtils;

namespace Pikagram.Controllers;

[Authorize]
[Route("user")]
public class UserController(AppDbContext db, UserManager<User> userManager, MediaService mediaService) : Controller
{
    [HttpPost("change-background")]
    public async Task<IActionResult> ChangeBackground(
        [FromForm, Bind(Prefix = nameof(ProfileViewModel.BackgroundForm))] ChangeBackgroundViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return LocalRedirect(GetProfileUrl());
        }
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        var saveResult = await mediaService.SaveMedia(model.Media, "backgrounds", user.Id.ToString());
        if (!saveResult.HasValue)
        {
            ModelState.AddModelError(string.Empty, "Neplatné pozadí");
            return LocalRedirect(GetProfileUrl());
        }

        if (saveResult.Value.type != MediaType.Image)
        {
            mediaService.RemoveMedia(saveResult.Value.relpath);
            ModelState.AddModelError(string.Empty, "Neplatný formát obrázku");
            return LocalRedirect(GetProfileUrl());
        }
        
        user.ChangeBackgroundSrc(WithRandomQueryParam(saveResult.Value.relpath));
        await db.SaveChangesAsync();
        return LocalRedirect(GetProfileUrl());
    }

    [HttpPost("change-avatar")]
    public async Task<IActionResult> ChangeAvatar(
        [FromForm, Bind(Prefix = nameof(ProfileViewModel.AvatarForm))] ChangeAvatarViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return LocalRedirect(GetProfileUrl());
        }
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        var saveResult = await mediaService.SaveMedia(model.Media, "avatars", user.Id.ToString());
        if (!saveResult.HasValue)
        {
            ModelState.AddModelError(string.Empty, "Neplatná profilovka");
            return LocalRedirect(GetProfileUrl());
        }

        if (saveResult.Value.type != MediaType.Image)
        {
            mediaService.RemoveMedia(saveResult.Value.relpath);
            ModelState.AddModelError(string.Empty, "Neplatný formát obrázku");
            return LocalRedirect(GetProfileUrl());
        }
        
        user.ChangeAvatarSrc(WithRandomQueryParam(saveResult.Value.relpath));
        await db.SaveChangesAsync();
        return LocalRedirect(GetProfileUrl());
    }

    [HttpPost("change-bio")]
    public async Task<IActionResult> ChangeBio(
        [FromForm, Bind(Prefix = nameof(ProfileViewModel.BioForm))] ChangeBioViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return LocalRedirect(GetProfileUrl());
        }
        
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        user.ChangeBio(model.Bio);
        await db.SaveChangesAsync();
        return LocalRedirect(GetProfileUrl());
    }

    private string GetProfileUrl()
    {
        return $"/profile/{User.Identity?.Name ?? ""}";
    }
}