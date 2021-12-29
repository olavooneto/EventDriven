using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class PostServiceContext : DbContext
    {
        public PostServiceContext(DbContextOptions<PostServiceContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

    }
}