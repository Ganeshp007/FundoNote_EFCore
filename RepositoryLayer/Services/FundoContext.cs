namespace RepositoryLayer.Services
{
    using Microsoft.EntityFrameworkCore;
    using RepositoryLayer.Services.Entity;

    public class FundoContext : DbContext
    {
        public FundoContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Label> Labels { get; set; }
    }
}
