using System;
using System.ComponentModel.DataAnnotations;
using Project33.Data;
namespace Project33.Services.Models
{
    public class Favorites
    {
        public int id { get; set; }
        public int book_id { get; set; }
        public  int user_id { get; set; }
        public string book_name { get; set; }
    }
}