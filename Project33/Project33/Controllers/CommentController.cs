using System;
using Microsoft.AspNetCore.Mvc;

namespace Project33.Controllers
{
    public class CommentController : Controller
    {
        private DateTime getToday()
        {
            return DateTime.Now;
        }
    }
}