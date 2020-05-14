using Microsoft.EntityFrameworkCore;
using Project33.Services.Models;

namespace Project33.Data
{
    public class FavoritesContext : DbContext
    {
        public FavoritesContext(DbContextOptions<FavoritesContext> options) : base(options)
        {
        }

        public FavoritesContext()
        {
            Database.EnsureCreated();
        }
        public  DbSet<Favorites> Favorites { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Favorites;Username=postgres;Password=1923148");
        }

    }
}