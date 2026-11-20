using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pikagram.Models;
using Pikagram.Models.Comments;
using Pikagram.Models.Posts;

namespace Pikagram;

public class AppDbContext : IdentityDbContext
{
    // bez inicializace by tam zůstal null (v tomhle je EpickyORM blbuvzdornější)
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}