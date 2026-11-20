using Pikagram.Enums;

namespace Pikagram.Models;

public class Post
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public Guid AuthorId { get; }
    public string Description { get; private set; } = string.Empty;
    public string MediaSrc { get; private set; } = string.Empty;
    public MediaType MediaType { get; private set; }
    public virtual User Author { get; }
    public virtual List<Comment> Comments { get; } = [];
    public virtual List<Like> Likes { get; } = [];

    public Post()
    {
    }

    public Post(Guid authorId, string description, string mediaSrc, MediaType mediaType)
    {
        AuthorId = authorId;
        Description = description;
        MediaSrc = mediaSrc;
        MediaType = mediaType;
    }

    public void ChangeDescription(string description)
    {
        Description = description;
    }

    public void ChangeSrc(string mediaSrc, MediaType mediaType)
    {
        MediaSrc = mediaSrc;
        MediaType = mediaType;
    }
}