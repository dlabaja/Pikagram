namespace Pikogram.Models.Comments;

public class Comment : AbstractComment
{
    public virtual List<ReplyComment> Replies { get; } = [];

    public Comment()
    {
    }

    public Comment(User author, string content, DateTime datePublished) : base(author, content, datePublished)
    {
    }

    public void Reply(User author, string content)
    {
        Replies.Add(new ReplyComment(this, author, content, DateTime.UtcNow));
    }
}