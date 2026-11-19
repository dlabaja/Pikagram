namespace Pikogram.Models.Comments;

public class Comment : AbstractComment
{
    public List<ReplyComment> Replies { get; }
    
    public Comment(User author, string content, DateTime datePublished,
        List<ReplyComment> replies) : base(author, content, datePublished)
    {
        Replies = replies;
    }
    
    public void Reply(User author, string content)
    {
        Replies.Add(new ReplyComment(this, author, content, DateTime.UtcNow));
    }
}