using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pikagram.Models;

namespace Pikagram;

// 1:1 -> jeden záznam patří k právě jednomu záznamu a naopak
// 1:N -> jeden záznam ukazuje na více jiných záznamů, každý z nich má jen jednoho rodiče
//        Autor <-> Post, Autor má N postů, každý Post má 1 autora
//        Post <-> Komentář, Post má N komentářů, každý komentář je pod jedním Postem
// M:N -> mnoho záznamů ukazuje na více jiných záznamů, too complicated
//        User <-> User, každý User má M sledujících a každý User N lidí sleduje

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    // bez inicializace by tam zůstal null (v tomhle je EpickyORM blbuvzdornější)
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Follower> Followers => Set<Follower>();
    public DbSet<Message> Messages => Set<Message>();


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // identity vazby

        builder.Entity<Follower>(entity =>
        {
            // složený klíč
            entity.HasKey(x => new { x.FollowerId, x.FollowingId });
            // Vazba Follower <-> User
            // Follower ukazuje na 1 Usera (HasOne)
            // User ukazuje na N Followerů -> 1:N (WithMany)
            entity.HasOne(x => x.FollowerUser)
                .WithMany(x => x.Following)
                .HasForeignKey(x => x.FollowerId); // nastavuje klíč pro vazbu
            entity.HasOne(x => x.FollowingUser)
                .WithMany(x => x.Followers)
                .HasForeignKey(x => x.FollowingId);
        });

        builder.Entity<Like>(entity =>
        {
            entity.HasKey(x => new { x.UserId, x.LikedId, x.LikedType });
            // Like <-> User
            // Like ukazuje na 1 Usera, User ukazuje na N liků -> 1:N 
            entity.HasOne(x => x.User)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.UserId);
        });

        builder.Entity<Post>(entity =>
        {
            entity.HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.AuthorId);
            entity.HasMany(x => x.Likes)
                .WithOne()
                .HasForeignKey(x => x.LikedId);
        });

        builder.Entity<Comment>(entity =>
        {
            entity.HasOne(x => x.Author)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.AuthorId);
            entity.HasOne(x => x.Parent)
                .WithMany()
                .HasForeignKey(x => x.ParentId);
            entity.HasMany(x => x.Likes)
                .WithOne()
                .HasForeignKey(x => x.LikedId);
        });
        
        builder.Entity<Message>(entity =>
        {
            // Message <-> User
            // Zpráva má 1 Usera, Usera má N zpráv
            entity.HasOne(x => x.Sender)
                .WithMany()
                .HasForeignKey(x => x.SenderId);
            entity.HasOne(x => x.Receiver)
                .WithMany()
                .HasForeignKey(x => x.ReceiverId);
        });
        
        // user už porpojuju z druhé strany, nemusím ho definovat
    }
}