using Microsoft.EntityFrameworkCore;
using Project33.Services.Models;

namespace Project33.Data
{
    public class CommentContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        
        public CommentContext(DbContextOptions<CommentContext> options)
            : base(options)
        {
        }
        
        public CommentContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WebAppComments;Username=postgres;Password=85ilasin85");
        }
    }
}