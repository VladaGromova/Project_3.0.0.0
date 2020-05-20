using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project33.Services.Models;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;//System.Web.Mvc.ActionResult
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using Project33.Data;
using Microsoft.AspNet.Identity;
using Project33.Services;
using Project33.Controllers;

namespace Project33.Controllers
{
    public class BooksController : Controller
    {
        BooksContext db = new BooksContext();
        private BooksContext _booksContext;
        
        public List<Books> BookService()
        {
            _booksContext = new BooksContext();
            return _booksContext.Books.Select(b => new Books()
            {
                id = b.id,
                name = b.name,
                author = b.author,
                genre = b.genre,
                description = b.description,
                cover = b.cover,
                likes = b.likes
            }).ToList();
        }
        
        public ActionResult Index()
        {
            return View(db.Books);
        }

        // public ActionResult IndexForUsers()
        // {
        //     return View(db.Books);
        // }

        public ActionResult BookPage(int? id)
        {
            var b = db.Books.Find(id);
            return View(b);
        }
        /*public async Task<IActionResult> BookPageForUsers(int? id)
        {
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found

            FavoritesContext favor_db = new FavoritesContext();
            
            LikesContext likes_db = new LikesContext();
            var num_of_likes = likes_db.Likes.ToArray().Length;

            Books b = db.Books.FirstOrDefault(b => b.id == id);
            if (likes_db.Likes.FirstOrDefault(i => (i.user_id == user.Id) && (i.book_id == id)) == null)
            {
                //не поставлен лайк
                if (favor_db.Favorites.FirstOrDefault(f => (f.user_id==user.Id))==null){
                    // нет лайка нет избранного
                return View(b);
                }
                else
                {
                    // нет лайка есть избранное
                    return RedirectToAction("FavoredBookPageForUsers", "Books", new{b_id = id});
                }
            }
            else
            {
                // поставлен лайк
                if (favor_db.Favorites.FirstOrDefault(f => (f.user_id==user.Id))!=null)
                {
                    //поставлен лайк есть избранное
                    return RedirectToAction("LikedFavoredBookPageForUsers", "Books", new{b_id = id});
                    
                }

                // поставлен лайк нет избранного
                return RedirectToAction("LikedBookPageForUsers", "Books", new{b_id = id});
            }
        }*/

        // public IActionResult LikedBookPageForUsers(int b_id)
        // {
        //     
        //     var b = db.Books.Find(b_id);
        //     return View(b);
        // }
        
        /*public IActionResult LikedFavoredBookPageForUsers(int b_id)
        {
            
            var b = db.Books.Find(b_id);
            return View(b);
        }
        
        public IActionResult FavoredBookPageForUsers(int b_id)
        {
            
            var b = db.Books.Find(b_id);
            return View(b);
        }*/
        
        

        public ActionResult GenrePage(string? genre)
        {
            BookService bookService=new BookService();
            var books = from b in db.Books select b;
            books = books.Where(b => b.genre.Contains(genre));
            return View(books.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string searchString)
        {
            var books = from b in db.Books select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.name.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }
        // [HttpPost]
        // public async Task<IActionResult> IndexForUsers(string searchString)
        // {
        //     var books = from b in db.Books select b;
        //     if (!String.IsNullOrEmpty(searchString))
        //     {
        //         books = books.Where(b => b.name.Contains(searchString));
        //     }
        //
        //     return View(await books.ToListAsync());
        // }

        [HttpPost]
        public async void ToFavor(int bookId)
        {
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found

            FavoritesContext _db = new FavoritesContext();
            var real_num_of_favs = _db.Favorites.ToArray().Length;
            
            Books b = db.Books.FirstOrDefault(b => b.id == bookId);

            Favorites fav = new Favorites()
            {
                id = (++real_num_of_favs),
                book_id = bookId,
                user_id = user.Id,
                book_name = b.name
            };
            _db.Favorites.Add(fav);
            await _db.SaveChangesAsync();    
        }

        [HttpPost]
        public async void ToLike(int num_of_likes, int bookId)
        {
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found

            LikesContext likes_db = new LikesContext();
            var real_num_of_likes = likes_db.Likes.ToArray().Length;

            Books b = db.Books.FirstOrDefault(b => b.id == bookId);
                // если в бд лайков не найден ни один лайк от пользователя
                Likes like = new Likes()
                {
                    id = (++real_num_of_likes),
                    book_id = bookId,
                    user_id = user.Id
                };

                likes_db.Likes.Add(like);
                
                b.likes = num_of_likes;

                await db.SaveChangesAsync();
            await likes_db.SaveChangesAsync(); 
        }

        [HttpPost]
        public async void DeleteFavor(int bookId)
        {
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found

            FavoritesContext _db = new FavoritesContext();
            var real_num_of_favs = _db.Favorites.ToArray().Length;

            Books b = db.Books.FirstOrDefault(b => b.id == bookId);
            
            var favor_to_remove = _db.Favorites.FirstOrDefault(i => (i.user_id == user.Id) && (i.book_id == bookId));
            _db.Remove(favor_to_remove);
            await _db.SaveChangesAsync(); 
        }

        [HttpPost]
        public async void ToUnLike(int num_of_likes, int bookId)
        {
            
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found

            LikesContext likes_db = new LikesContext();
            var real_num_of_likes = likes_db.Likes.ToArray().Length;

            Books b = db.Books.FirstOrDefault(b => b.id == bookId);
            
            var like_to_remove = likes_db.Likes.FirstOrDefault(i => (i.user_id == user.Id) && (i.book_id == bookId));
            likes_db.Remove(like_to_remove);
                
            b.likes = --num_of_likes;
        
        await db.SaveChangesAsync();
        await likes_db.SaveChangesAsync(); 
        }

        [HttpPost]
        public async void UpdateBooksLikes(int number, int bookId)
        {
            
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found
            
            LikesContext likes_db = new LikesContext();
            var num_of_likes = likes_db.Likes.ToArray().Length;

                Books b = db.Books.FirstOrDefault(b => b.id == bookId);
            if (likes_db.Likes.FirstOrDefault(i =>(i.user_id== user.Id)&&(i.book_id==bookId)) == null)
            {
                // если в бд лайков не найден ни один лайк от пользователя
                Likes like = new Likes()
                {
                    id = (++num_of_likes),
                    book_id = bookId,
                    user_id = user.Id
                };

                likes_db.Likes.Add(like);
                
                b.likes = number;
            }
            else
            {
                // в бд лайков есть лайк от пользователя на заданную книгу

                var like_to_remove = likes_db.Likes.FirstOrDefault(i => (i.user_id == user.Id) && (i.book_id == bookId));
                likes_db.Remove(like_to_remove);
                
                //Books b = db.Books.FirstOrDefault(b => b.id == bookId);
                b.likes = (number-2);
            }
            await db.SaveChangesAsync();
            await likes_db.SaveChangesAsync(); 
        }
        
    }
}
