using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SecurityPeerGraded.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
