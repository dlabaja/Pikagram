using System.ComponentModel.DataAnnotations;

namespace Pikagram.Models;

public class Comment
{
    public Guid Id { get; } = Guid.CreateVersion7(); // obsahuje čas
    public Guid AuthorId { get; }
    public Guid? ParentId { get; }
    [MaxLength(250)]
    public string Content { get; private set; } = "";
    public DateTime DatePublished { get; } = DateTime.UtcNow; // nebude bordel v časových zónách
    public bool IsEdited { get; private set; }
    public virtual User Author { get; } = null;
    public virtual Comment? Parent { get; } = null;
    public virtual List<Like> Likes { get; } = [];

    public Comment()
    {
    }

    public Comment(Guid authorId, string content, DateTime datePublished, Guid? parentId)
    {
        AuthorId = authorId;
        Content = content;
        DatePublished = datePublished;
        ParentId = parentId;
    }

    public void Edit(string newContent)
    {
        Content = newContent;
        IsEdited = true;
    }
}