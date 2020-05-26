namespace Project33.Services.Models
{
    public class Likes
    {
       public int id { get; set; }
       public int book_id { get; set; }
       public  int user_id { get; set; }
       public bool is_like { get; set; }
    }
}