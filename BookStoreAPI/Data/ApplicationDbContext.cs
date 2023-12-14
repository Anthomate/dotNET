using BookStoreAPI.Entities.BookEntities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentDir = Directory.GetCurrentDirectory();

            var dbPath = Path.Combine(currentDir, "bookstore.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        public DbSet<Book> Books { get; set; } = default!;
        public DbSet<Author> Authors { get; set; } = default!;
        public DbSet<Publisher> Publishers { get; set; } = default!;
        public DbSet<Genre> Genres { get; set; } = default!;
    }
}