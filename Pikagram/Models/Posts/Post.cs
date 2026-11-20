using Pikagram.Models.Comments;

namespace Pikagram.Models.Posts;

public class Post
{
    public User User { get; }
    public List<Comment> Comments { get; } = [];
}