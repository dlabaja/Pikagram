using Pikogram.Models.Comments;

namespace Pikogram.Models;

public class User
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public string Username { get; }
    public List<Like> Likes { get; } = [];

    public User(string username)
    {
        Username = username;
    }
}