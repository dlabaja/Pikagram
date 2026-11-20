using Microsoft.AspNetCore.Identity;

namespace Pikagram.Models;

public class User : IdentityUser
{
    public string Bio { get; } = string.Empty;
    public string ProfilePictureSrc { get; } = string.Empty;
    public string BackgroundSrc { get; } = string.Empty;
    public virtual List<Like> Likes { get; } = [];
    public virtual List<Post> Posts { get; } = [];
    public virtual List<Follower> Followers { get; } = [];
    public virtual List<Follower> Following { get; } = [];
    public virtual List<Comment> Comments { get; } = [];

    public User()
    {
    }
}