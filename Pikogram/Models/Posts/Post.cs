using Pikogram.Models.Comments;

namespace Pikogram.Models.Posts;

public class Post
{
    public User User { get; }
    public List<Comment> Comments { get; } = [];
}