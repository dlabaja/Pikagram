using Pikogram.Models.Comments;

namespace Pikogram.Models;

public class User
{
    public Guid Id { get; }
    public string Username { get; }
    public List<Like> Likes { get; }

    public User(string username, List<Like> likes)
    {
        Id = Guid.CreateVersion7();
        Username = username;
        Likes = likes;
    }
}