using System;
using System.ComponentModel.DataAnnotations;
using Project33.Data;
namespace Project33.Services.Models
{
    public class Books
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
    }
}
