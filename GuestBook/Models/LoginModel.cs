﻿using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
    public class LoginModel
    {
        // имя пользователя
        [Required(ErrorMessage = "Запишите имя пользователя")]
        [RegularExpression(@"^[A-Za-z\d\s]{1,50}$", ErrorMessage = "Имя может содержать только латинские буквы (большие и маленькие), цифры и пробелы")]
        [StringLength(50)]
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }

        // пароль пользователя
        [Required(ErrorMessage = "Введите пароль")]
        //[StringLength(20,MinimumLength =8,ErrorMessage ="Пароль должен быть не менее 8 и не более 20 символов")]        
        [RegularExpression(@"^[A-Za-z\d]{8,20}$", ErrorMessage = "Пароль должен содержать только латинские буквы (большие и маленькие) и цифры")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        ////==============================================================
        //// альтернативный вариант

        //[Required]
        //public string? Name { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string? Password { get; set; }
    }
}
