using ReadSynch.Models;
using Microsoft.EntityFrameworkCore;

namespace ReadSynch.Data
{
    public class ReadSynchContext : DbContext
    {
        public ReadSynchContext(DbContextOptions<ReadSynchContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<UserBook>().ToTable("UserBook");
        }
    }
}
