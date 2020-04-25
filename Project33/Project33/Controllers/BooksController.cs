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
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                Genre = b.Genre,
                Description = b.Description
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

        [HttpPost]
        public async Task<IActionResult> Index(string searchString)
        {
            var books = from b in db.Books select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Name.Contains(searchString));
                //books = books.Where(s => s.Author.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> IndexForUsers(string searchString)
        {
            var books = from b in db.Books select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Name.Contains(searchString));
                //books = books.Where(s => s.Author.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }
        
    }
}
