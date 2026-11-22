using System.ComponentModel.DataAnnotations;

namespace Pikagram.Models;

public class Comment
{
    public Guid Id { get; private set; } = Guid.CreateVersion7(); // obsahuje čas
    public Guid AuthorId { get; private set; }
    public Guid PostId { get; private set; }
    [MaxLength(250)] 
    public string Text { get; private set; } = string.Empty;
    public DateTime DatePublished { get; private set; } = DateTime.UtcNow; // nebude bordel v časových zónách
    public bool IsEdited { get; private set; }
    public virtual User Author { get; private set; }
    public virtual Post Post { get; private set; }

    public Comment()
    {
    }

    public Comment(Guid authorId, Guid postId, string text, DateTime datePublished)
    {
        AuthorId = authorId;
        PostId = postId;
        Text = text;
        DatePublished = datePublished;
    }

    public void Edit(string newContent)
    {
        Text = newContent;
        IsEdited = true;
    }
}