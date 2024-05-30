using System.ComponentModel.DataAnnotations;

namespace GuestBook.Models
{
	public class Message
	{
		// идентификатор сообщения
		public int Id { get; set; }

		// текст сообщения
		[Required(ErrorMessage = "Вы не можете отправить пустое сообщение")]
		[StringLength(1000, ErrorMessage = "Сообщение не может быть более 1000 символов")]
		public string? MessageContent { get; set; }

		// время и дата сообщения
		public DateTime MessageDate { get; set; }

		// внешний ключ для связи с пользователем, который отправил сообщение
		public int UserId { get; set; }

		// навигационное свойство для доступа к пользователю, которому принадлежит сообщение, сообщение может быть привязано только к одному пользователю
		public User? User { get; set; }		
	}
}
