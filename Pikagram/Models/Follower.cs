namespace Pikagram.Models;

public class Follower
{
    // pk je (FollowerId, FollowingId)
    // já (follower) (sleduju) jeho (following)
    public Guid FollowerId { get; private set; }
    public Guid FollowingId { get; private set; }
    public virtual User FollowerUser { get; private set; }
    public virtual User FollowingUser { get; private set; }
}