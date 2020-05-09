using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project33.Data;
using Project33.Models;
using Project33.Services.Models;
using Microsoft.AspNet.Identity;

namespace Project33.Controllers
{
    public class CommentController : Controller
    {
        CommentContext com_db = new CommentContext();
        UserContext user_db = new UserContext();
        BooksContext book_db = new BooksContext();
        
        public ActionResult Comments(PostDataModel bookId)
        {
            IEnumerable<Comment> comments = com_db.Comments.Where(comments => comments.BookId == bookId.Number);
            return View(comments);
        }

        [HttpPost]
        public async Task<ActionResult> NewComment(PostDataModel model)
        {
            var userName = User.Identity.GetUserName();
            User user = await user_db.Users.FirstOrDefaultAsync(x => x.Login == userName);
            
            Comment newComment = new Comment { BookId = model.Number, UserId = user.Id, Text = model.Text, CreatedDate = DateTime.Now};
            
            com_db.Comments.Add(newComment);
                        
            await com_db.SaveChangesAsync();

            return View(newComment);
        }
    } 
}