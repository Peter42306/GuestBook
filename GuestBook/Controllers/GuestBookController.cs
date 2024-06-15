using GuestBook.Models;
using GuestBook.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
        //MyLoggerTXT _myLoggerTxt;

        //public GuestBookController(GuestBookContext context,MyLoggerTXT myLoggerTxt)
        public GuestBookController(GuestBookContext context)
        {
            _context = context;
            //_myLoggerTxt = myLoggerTxt;
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

            //// Записывает все сообщения в файл
            //// Формируем строку для логгирования
            //StringBuilder logMessage = new StringBuilder();
            //logMessage.AppendLine("Сообщение получено из БД:");

            //foreach (var message in messageContext)
            //{
            //    logMessage.Append($"Message Id: {message.Id}, ");
            //    logMessage.Append($"User: {message.User.Name}, ");
            //    logMessage.Append($"Date: {message.MessageDate}, ");
            //    logMessage.Append($"Text: {message.MessageContent}, ");
            //    logMessage.AppendLine($"");
            //}

            //// Логируем сформированную строку
            //_myLoggerTxt.Log(logMessage.ToString());

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
