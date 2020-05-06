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

        public ActionResult IndexForUsers()
        {
            return View(db.Books);
        }

        public ActionResult BookPage(int? id)
        {
            var b = db.Books.Find(id);
            return View(b);
        }
        public ActionResult BookPageForUsers(int? id)
        {
            var b = db.Books.Find(id);
            return View(b);
        }

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
        [HttpPost]
        public async Task<IActionResult> IndexForUsers(string searchString)
        {
            var books = from b in db.Books select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.name.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }
        

        [HttpPost]
        public async void UpdateBooksLikes(int number, int bookId)
        {
            
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found
            
            LikesContext likes_db = new LikesContext();
            var num_of_likes = likes_db.Likes.ToArray().Length;

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
                
                Books b1 = db.Books.FirstOrDefault(b1 => b1.id == bookId);
                b1.likes = number;
            }
            else
            {
                // в бд лайков есть лайк ИЛИ ЛАЙКИ от пользователя на заданную книгу

                var like_to_remove = likes_db.Likes.FirstOrDefault(i => (i.user_id == user.Id) && (i.book_id == bookId));
                likes_db.Remove(like_to_remove);
                
                Books b2 = db.Books.FirstOrDefault(b2 => b2.id == bookId);
                b2.likes = (number-2);
            }
            

            Books b = db.Books.FirstOrDefault(b => b.id == bookId);
            b.likes = number;
            //db.SaveChanges();
            await db.SaveChangesAsync();
            await likes_db.SaveChangesAsync();
        }
    }
}
