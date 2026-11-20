using System.ComponentModel.DataAnnotations;

namespace Pikagram.Models.Comments;

public class Comment
{
    public Guid Id { get; } = Guid.CreateVersion7(); // obsahuje čas
    [MaxLength(250)]
    public string Content { get; private set; } = "";
    public DateTime DatePublished { get; } = DateTime.UtcNow; // nebude bordel v časových zónách
    public bool IsEdited { get; private set; } = false;
    public virtual User Author { get; } = null;
    public virtual Comment? Parent { get; } = null;
    public virtual List<Like> Likes { get; } = [];

    public Comment()
    {
    }

    public Comment(User author, string content, DateTime datePublished, Comment? parent)
    {
        Author = author;
        Content = content;
        DatePublished = datePublished;
        Parent = parent;
    }

    public void Edit(string newContent)
    {
        Content = newContent;
        IsEdited = true;
    }
}