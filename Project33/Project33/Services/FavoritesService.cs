using System.Collections.Generic;
using System.Linq;
using Project33.Data;
using Project33.Services.Models;

namespace Project33.Services
{
    public class FavoritesService
    {
        private readonly FavoritesContext _FavoritesContext;

        public FavoritesService()
        {
            _FavoritesContext = new FavoritesContext();
        }

        public List<Favorites> GetFavorites()
        {
            return _FavoritesContext.Favorites.Select(BuildFavor).ToList();
        }
       
        private Favorites BuildFavor(Favorites f)
        {
            return new Favorites()
            {
                id = f.id,
                book_id = f.book_id,
                user_id = f.user_id
            };
        }
    }
}