using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project33.Models;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;//System.Web.Mvc.ActionResult
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using Project33.Data;
using Project33.Services;

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
                description = b.description
            }).ToList();
        }
        
        public ActionResult Index()
        {
            return View(db.Books);

        }

        public ActionResult BookPage(int? id)
        {
            var b = db.Books.Find(id);
            return View(b);
        }
    }
}
