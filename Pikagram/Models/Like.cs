using Pikagram.Enums;

namespace Pikagram.Models;

public class Like
{
    public Guid UserId { get; }
    public Guid LikedId { get; }
    public LikedType LikedType { get; }
    public virtual User User { get; }

    public Like()
    {
    }

    public Like(Guid userId, Guid likedId, LikedType likedType)
    {
        UserId = userId;
        LikedId = likedId;
        LikedType = likedType;
    }
}