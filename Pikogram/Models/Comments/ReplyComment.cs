namespace Pikogram.Models.Comments;

public class ReplyComment : AbstractComment
{
    public Comment Comment { get; }
    
    public ReplyComment(Comment comment, User author, string content, DateTime datePublished) : base(author, content, datePublished)
    {
        Comment = comment;
    }

    public void Reply(ReplyComment reply, User author, string content)
    {
        Comment.Replies.Add(new ReplyComment(Comment, author, $"@{reply.Author.Username} {content}", DateTime.UtcNow));
    }
}