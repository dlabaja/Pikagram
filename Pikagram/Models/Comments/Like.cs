using Pikagram.Enums;

namespace Pikagram.Models.Comments;

public class Like
{
    public Guid UserId { get; }
    public Guid LikedId { get; }
    public LikedType LikedType { get; }

    public Like(Guid userId, Guid commentId, LikedType likedType)
    {
        UserId = userId;
        LikedId = commentId;
        LikedType = likedType;
    }
}