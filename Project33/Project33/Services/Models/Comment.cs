using System;

namespace Project33.Services.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Text { get; set; }
        
        public DateTime? CreatedDate { get; set; }
    }
}