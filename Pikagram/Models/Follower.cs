namespace Pikagram.Models;

public class Follower
{
    public Guid FollowerId { get; }
    public Guid FollowingId { get; }
    public virtual User FollowerUser { get; }
    public virtual User FollowingUser { get; }
}