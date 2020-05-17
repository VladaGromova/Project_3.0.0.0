using System.Collections.Generic;

namespace Project33.Services.Models
{
    public class UserActionsInfo
    {
        public string username { get; set; }
        public List<Favorites> favors { get; set; }
        public List<Likes> likes { get; set; }
    }
}