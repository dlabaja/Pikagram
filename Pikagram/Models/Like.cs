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
}