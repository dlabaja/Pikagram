using Pikagram.Enums;

namespace Pikagram.Models;

public class Post
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public Guid AuthorId { get; }
    public MediaType MediaType { get; }
    public string MediaSrc { get; } = string.Empty;
    public virtual User Author { get; }
    public virtual List<Comment> Comments { get; } = [];
    public virtual List<Like> Likes { get; } = [];
}