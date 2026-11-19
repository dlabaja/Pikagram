namespace Pikogram.Models.Comments;

public abstract class AbstractComment
{
    public Guid Id { get; } = Guid.CreateVersion7(); // obsahuje čas
    public virtual User Author { get; } = null;
    public string Content { get; private set; } = "";
    public DateTime DatePublished { get; } = DateTime.UtcNow; // nebude bordel v časových zónách
    public bool IsEdited { get; private set; } = false;
    public virtual List<Like> Likes { get; } = [];

    protected AbstractComment()
    {
    }

    protected AbstractComment(User author, string content, DateTime datePublished)
    {
        Author = author;
        Content = content;
        DatePublished = datePublished;
    }

    public void Edit(string newContent)
    {
        Content = newContent;
        IsEdited = true;
    }
}