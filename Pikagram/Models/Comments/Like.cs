namespace Pikogram.Models.Comments;

public class Like
{
    public Guid UserId { get; }
    public Guid CommentId { get; }

    public Like(Guid userId, Guid commentId)
    {
        UserId = userId;
        CommentId = commentId;
    }
}