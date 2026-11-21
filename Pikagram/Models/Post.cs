using System.ComponentModel.DataAnnotations;
using Pikagram.Enums;

namespace Pikagram.Models;

public class Post
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public Guid AuthorId { get; }
    [MaxLength(250)]
    public string Text { get; private set; } = string.Empty;
    [MaxLength(250)]
    public string Tags { get; private set; } = string.Empty;
    [MaxLength(250)]
    public string MediaSrc { get; private set; } = string.Empty;
    public MediaType MediaType { get; private set; }
    public virtual User Author { get; }
    public virtual List<Comment> Comments { get; } = [];
    public virtual List<Like> Likes { get; } = [];

    public Post()
    {
    }

    public Post(Guid authorId, string text, string mediaSrc, MediaType mediaType)
    {
        AuthorId = authorId;
        Text = text;
        MediaSrc = mediaSrc;
        MediaType = mediaType;
    }

    public void ChangeText(string text)
    {
        Text = text;
    }

    public void ChangeSrc(string mediaSrc, MediaType mediaType)
    {
        MediaSrc = mediaSrc;
        MediaType = mediaType;
    }

    public void ChangeTags(string tags)
    {
        Tags = tags;
    }
}