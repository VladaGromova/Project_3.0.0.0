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
            Books b = db.Books.FirstOrDefault(b => b.id == bookId);
            b.likes = number;
            db.SaveChanges();
            db.SaveChangesAsync();
            
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName);
        }
    }
}
