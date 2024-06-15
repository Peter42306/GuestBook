using GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Controllers
{
    /// <summary>
    /// Контроллер для управления сообщениями гостевой книги
	/// _context для хранения контекста базы данных
	/// Конструктор контроллера, принимает контекст базы данных в качестве зависимости
    /// </summary>

    public class GuestBookController : Controller
	{
		private readonly GuestBookContext _context;

        public GuestBookController(GuestBookContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод для отображения всех сообщений гостевой книги (новые сообщения вверху списка)
        /// </summary>
        /// <returns>Возвращает представление для отображения сообщений гостевой книги с переданным контекстом сообщений</returns>
        public async Task<IActionResult> Index()
        {
            // Получаем список сообщений из базы данных, включая информацию о пользователях сортируем их в порядке убывания по дате сообщения и преобразуем результат в список
            var messageContext = _context.Messages
                .Include(m => m.User)
                .OrderByDescending(m=>m.MessageDate)
                .ToList();
            return View(messageContext); // Возвращает представление для отображения сообщений гостевой книги с переданным контекстом сообщений            
        }

        /*
        /// <summary>
        /// Метод для отображения всех сообщений гостевой книги (новые сообщения внизу списка)
        /// </summary>
        /// <returns>Возвращает представление для отображения сообщений гостевой книги с переданным контекстом сообщений</returns>
        public async Task<IActionResult> Index()
		{
			var messageContext = _context.Messages.Include(m => m.User); // Получаем все сообщения из базы данных вместе с соответствующими пользователями			
            return View(messageContext); // Возвращает представление для отображения сообщений гостевой книги с переданным контекстом сообщений            
        }
        */        
    }
}
