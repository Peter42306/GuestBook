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
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }

		// пароль пользователя		
		public string Password { get; set; }

		// коллекция сообщений созданных пользователем
		public ICollection<Message> Messages { get; set; }

		// ???
		public string Salt {  get; set; }
	}
}
