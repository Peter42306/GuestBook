namespace GuestBook.Models
{
    public class CreateMessageViewModel
    {
        public Message NewMessage { get; set; } // Новое сообщение для создания
        public IEnumerable<Message> Messages { get; set; } // Список предыдущих сообщений
    }
}
