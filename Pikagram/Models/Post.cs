using System.ComponentModel.DataAnnotations;
using Pikagram.Enums;

namespace Pikagram.Models;

public class Post
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid AuthorId { get; private set; }
    [MaxLength(250)]
    public string Text { get; private set; } = string.Empty;
    [MaxLength(250)]
    public string Tags { get; private set; } = string.Empty;
    [MaxLength(250)] 
    public string MediaSrc { get; private set; } = string.Empty;
    public MediaType MediaType { get; private set; }
    public DateTime DatePublished { get; private set; } = DateTime.UtcNow;
    public virtual User Author { get; private set; }
    public virtual List<Comment> Comments { get; private set; } = [];

    public Post()
    {
    }

    public Post(Guid authorId, string text, string tags, string mediaSrc, MediaType mediaType)
    {
        AuthorId = authorId;
        Text = text;
        Tags = tags;
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

    public string FormatTags()
    {
        if (string.IsNullOrWhiteSpace(Tags))
        {
            return "";
        }
        return string.Join(" ", Tags.Split(",")
            .Select(x => $"#{x.Trim()}"));
    }
}