using System.ComponentModel.DataAnnotations;

namespace Project33.Models
{
    public class EditViewModel
    {
        [Required(ErrorMessage ="Не указан логин")]
        public string Login { get; set; }
        
        [Required(ErrorMessage ="Не указан возраст")]
        public int Age { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}