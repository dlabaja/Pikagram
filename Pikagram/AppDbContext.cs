using Microsoft.EntityFrameworkCore;
using Pikogram.Models;
using Pikogram.Models.Comments;
using Pikogram.Models.Posts;

namespace Pikogram;

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