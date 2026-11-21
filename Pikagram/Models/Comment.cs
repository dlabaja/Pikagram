using System.ComponentModel.DataAnnotations;

namespace Pikagram.Models;

public class Comment
{
    public Guid Id { get; } = Guid.CreateVersion7(); // obsahuje čas
    public Guid AuthorId { get; }
    public Guid? ParentId { get; }
    [MaxLength(250)]
    public string Text { get; private set; } = string.Empty;
    public DateTime DatePublished { get; } = DateTime.UtcNow; // nebude bordel v časových zónách
    public bool IsEdited { get; private set; }
    public virtual User Author { get; }
    public virtual Comment? Parent { get; }
    public virtual List<Like> Likes { get; } = [];

    public Comment()
    {
    }

    public Comment(Guid authorId, string text, DateTime datePublished, Guid? parentId)
    {
        AuthorId = authorId;
        Text = text;
        DatePublished = datePublished;
        ParentId = parentId;
    }

    public void Edit(string newContent)
    {
        Text = newContent;
        IsEdited = true;
    }
}