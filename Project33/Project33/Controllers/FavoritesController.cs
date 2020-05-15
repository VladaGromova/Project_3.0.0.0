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
    public class FavoritesController : Controller
    {
        FavoritesContext db = new FavoritesContext();
        private FavoritesContext _FavoritesContext;
        
        public List<Favorites> FavoritesService()
        {
            _FavoritesContext = new FavoritesContext();
            return _FavoritesContext.Favorites.Select(b => new Favorites()
            {
                id = b.id,
                book_id = b.book_id,
                user_id = b.user_id
            }).ToList();
        }
    }
}