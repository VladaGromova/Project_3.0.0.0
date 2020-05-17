using System.Collections.Generic;
using System.Linq;
using Project33.Data;
using Project33.Services.Models;

namespace Project33.Services
{
    public class LikesService
    {
        private readonly LikesContext _likesContext;

        public LikesService()
        {
            _likesContext = new LikesContext();
        }

        public List<Likes> GetLikes()
        {
            return _likesContext.Likes.Select(BuildLike).ToList();
        }
        
        public Likes FindByUser(int user)
        {
            return _likesContext.Likes.Select(BuildLike).FirstOrDefault(book => book.user_id == user);
        }
        
        
        private Likes BuildLike(Likes b)
        {
            return new Likes()
            {
                id = b.id,
                book_id = b.book_id,
                user_id = b.user_id,
                is_like = b.is_like
            };
        }
    }
}