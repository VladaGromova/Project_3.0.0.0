﻿﻿using System.ComponentModel.DataAnnotations;
 
namespace Project33.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage ="Не указан логин")]
        public string Login { get; set; }
        
        [Display(Name = "Email")]
        [Required(ErrorMessage ="Не указан Email")]
        public string Email { get; set; }
        
        [Display(Name = "Возраст")]
        [Required(ErrorMessage ="Не указан возраст")]
        public int Age { get; set; }
        
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Не указан пароль")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Введите корректный адрес почты")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Я прекрасен")]
        [Required(ErrorMessage = "Подтвердите, что вы прекрасны")]
        public bool? ImGorgeous { get; set; }
    }
}