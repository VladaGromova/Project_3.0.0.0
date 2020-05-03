using Microsoft.EntityFrameworkCore;
using Project33.Services.Models;

namespace Project33.Data
{
    public class LikesContext : DbContext
    {
        public LikesContext(DbContextOptions<LikesContext> options) : base(options)
        {
        }

        public LikesContext()
        {
            Database.EnsureCreated();
        }
        public  DbSet<Likes> Likes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Likes;Username=postgres;Password=1923148");
        }

    }
}