using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class UserServiceContext: DbContext
    {
        public UserServiceContext(DbContextOptions<UserServiceContext> options) :base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}