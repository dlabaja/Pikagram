using System.ComponentModel.DataAnnotations;

namespace Pikagram.Models;

public class Comment
{
    public Guid Id { get; private set; } = Guid.CreateVersion7(); // obsahuje čas
    public Guid AuthorId { get; private set; }
    public Guid? ParentId { get; private set; }
    [MaxLength(250)] 
    public string Text { get; private set; } = string.Empty;
    public DateTime DatePublished { get; private set; } = DateTime.UtcNow; // nebude bordel v časových zónách
    public bool IsEdited { get; private set; }
    public virtual User Author { get; private set; }
    public virtual Comment? Parent { get; private set; }
    public virtual List<Like> Likes { get; private set; } = [];

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