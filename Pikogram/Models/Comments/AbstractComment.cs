namespace Pikogram.Models.Comments;

public abstract class AbstractComment
{
    public Guid Id { get; } = Guid.CreateVersion7(); // obsahuje čas
    public User Author { get; } = null;
    public string Content { get; } = "";
    public DateTime DatePublished { get; } = DateTime.UtcNow; // nebude bordel v časových zónách
    public List<Like> Likes { get; } = [];

    protected AbstractComment()
    {
    }

    protected AbstractComment(User author, string content, DateTime datePublished)
    {
        Author = author;
        Content = content;
        DatePublished = datePublished;
    }
}