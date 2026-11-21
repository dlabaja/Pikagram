using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Pikagram.Models;

public class User : IdentityUser<Guid>
{
    private static readonly string _defaultAvatarSrc = "/media/avatars/default-avatar.webp";
    private static readonly string _defaultBackgroundSrc = "/media/backgrounds/default-background.webp";
    
    [MaxLength(50)]
    public string FirstName { get; set; }  = string.Empty;
    [MaxLength(50)]
    public string LastName { get; set; }  = string.Empty;
    public DateTime DateBorn { get; set; }
    [MaxLength(250)]
    public string Bio { get; private set; } = string.Empty;
    [MaxLength(250)]
    public string AvatarSrc { get; private set; } = _defaultAvatarSrc;
    [MaxLength(250)]
    public string BackgroundSrc { get; private set; } = _defaultBackgroundSrc;
    public virtual List<Like> Likes { get; } = [];
    public virtual List<Post> Posts { get; } = [];
    public virtual List<Follower> Followers { get; } = [];
    public virtual List<Follower> Following { get; } = [];
    public virtual List<Comment> Comments { get; } = [];

    public User()
    {
    }

    public void ChangeBio(string newBio)
    {
        Bio = newBio;
    }

    public void ChangeAvatarSrc(string src)
    {
        AvatarSrc = src;
    }
    
    public void ChangeBackgroundSrc(string src)
    {
        BackgroundSrc = src;
    }
}