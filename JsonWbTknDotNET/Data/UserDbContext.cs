using Microsoft.EntityFrameworkCore;
using JsonWbTknDotNET.Entities;

namespace JsonWbTknDotNET.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
