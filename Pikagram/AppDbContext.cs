using Microsoft.EntityFrameworkCore;
using Pikagram.Models;
using Pikagram.Models.Comments;
using Pikagram.Models.Posts;

namespace Pikagram;

public class AppDbContext : DbContext
{
    // bez inicializace by tam zůstal null (v tomhle je EpickyORM blbuvzdornější)
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseLazyLoadingProxies().UseSqlite("Data Source=database.sqlite");
    }
}