using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
	public class User
	{
		// инициализируется коллекция Messages типа HashSet<Message>(), используется для хранения сообщений пользователя
		public User()
		{
			this.Messages = new HashSet<Message>();
		}

		// идентификатор пользователя
		public int Id { get; set; }

		// имя пользователя
		[Required(ErrorMessage = "Запишите имя пользователя")]
		[RegularExpression(@"^[A-Za-z\d]{8,20}$", ErrorMessage = "Имя может содержать только латинские буквы (большие и маленькие) и цифры")]
		[StringLength(50)]
		[Display(Name ="Имя пользователя")]
		public string Name { get; set; }

		// пароль пользователя
		[Required(ErrorMessage = "Введите пароль")]
		//[StringLength(20,MinimumLength =8,ErrorMessage ="Пароль должен быть не менее 8 и не более 20 символов")]        
		[RegularExpression(@"^[A-Za-z\d]{8,20}$", ErrorMessage = "Пароль должен содержать только латинские буквы (большие и маленькие) и цифры")]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		// коллекция сообщений созданных пользователем
		public ICollection<Message> Messages { get; set; }
	}
}
