using Pikagram.Enums;

namespace Pikagram.Models;

public class Like
{
    public Guid UserId { get; private set; }
    public Guid LikedId { get; private set; }
    public LikedType LikedType { get; private set; }
    public virtual User User { get; private set; }

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