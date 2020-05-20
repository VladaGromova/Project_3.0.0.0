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
        
        FavoritesContext db_favors = new FavoritesContext();
        private FavoritesContext _favoritesContext;

        public async Task<IActionResult> Favored()
        {
            UserContext user_db = new UserContext();
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName); // UserId found

            var favors = from f in db_favors.Favorites select f;
            favors = favors.Where(f => f.user_id.Equals(user.Id));
            return View(favors.ToList());
        }
    }
}