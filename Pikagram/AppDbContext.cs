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

public class AppDbContext : IdentityDbContext
{
    // bez inicializace by tam zůstal null (v tomhle je EpickyORM blbuvzdornější)
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Follower> Followers => Set<Follower>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Follower>(entity =>
        {
            // složený klíč
            entity.HasKey(x => new { x.FollowerId, x.FollowingId });
            // Vazba Follower <-> User
            // Follower ukazuje na 1 Usera (HasOne)
            // User ukazuje na N Followerů -> 1:N (WithMany)
            entity.HasOne(x => x.FollowerUser)
                .WithMany()
                .HasForeignKey(x => x.FollowerId); // nastavuje klíč pro vazbu
            entity.HasOne(x => x.FollowingUser)
                .WithMany()
                .HasForeignKey(x => x.FollowingId);
        });

        builder.Entity<Like>(entity =>
        {
            entity.HasKey(x => new { x.UserId, x.LikedId });
            // Like <-> User
            // Like ukazuje na 1 Usera, User ukazuje na N liků -> 1:N 
            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        });

        builder.Entity<Post>(entity =>
        {
            entity.HasOne(x => x.Author)
                .WithMany()
                .HasForeignKey(x => x.AuthorId);
            // Příspěvek <-> Komentář
            // Příspěvek ukazuje na N komentářů, Komentář na 1 příspěvek
            // fk se vytvoří automaticky
            entity.HasMany(x => x.Comments)
                .WithOne();
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

        builder.Entity<User>(entity =>
        {
            entity.HasMany(x => x.Likes)
                .WithOne()
                .HasForeignKey(x => x.UserId);
            entity.HasMany(x => x.Posts)
                .WithOne()
                .HasForeignKey(x => x.AuthorId);
            entity.HasMany(x => x.Followers)
                .WithOne()
                .HasForeignKey(x => x.FollowerId);
            entity.HasMany(x => x.Following)
                .WithOne()
                .HasForeignKey(x => x.FollowingId);
            entity.HasMany(x => x.Comments)
                .WithOne()
                .HasForeignKey(x => x.AuthorId);
        });
    }
}