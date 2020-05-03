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
    public class LikesController : Controller
    {
        LikesContext db = new LikesContext();
        private LikesContext _likesContext;
        
        public List<Likes> LikesService()
        {
            _likesContext = new LikesContext();
            return _likesContext.Likes.Select(b => new Likes()
            {
                id = b.id,
                book_id = b.book_id,
                user_id = b.user_id,
                is_like = b.is_like
            }).ToList();
        }
    }
}
