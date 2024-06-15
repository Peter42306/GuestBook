using GuestBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    /// <summary>
    /// Контроллер для управления сообщениями
    /// _context - поле для хранения контекста базы данных
    /// Конструктор контроллера, принимает контекст базы данных в качестве зависимости
    /// </summary>
    public class MessagesController : Controller
    {
        private readonly GuestBookContext _context;

        public MessagesController(GuestBookContext context)
        {
            _context = context;
        }

        // GET: Message/Create
        /// <summary>
        /// Представление для создания нового сообщения
        /// </summary>
        /// <returns>Представление для создания нового сообщения</returns>
        [Authorize] // Атрибут для требования аутентификации перед доступом к этому действию
        public IActionResult Create()
        {
            ViewData["Name"] = HttpContext.Session.GetString("Name"); // Передаем в представление имя пользователя из сессии
            return View(); // Представление для создания нового сообщения
        }

        // POST: Message/Create
        /// <summary>
        /// POST запрос для создания нового сообщения
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // Атрибут для требования аутентификации перед доступом к этому действию
        public IActionResult Create([Bind("MessageContent")] Message message)
        {
            // Проверяем, прошла ли модель валидацию
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name; // Получаем имя пользователя из аутентификационных данных
                var user = _context.Users.SingleOrDefault(u => u.Name == userName); // Находим пользователя в базе данных

                // Если пользователь найден
                if (user != null)
                {
                    message.UserId = user.Id; // Устанавливаем ID пользователя и дату сообщения
                    message.MessageDate= DateTime.Now;

                    _context.Messages.Add(message); // Добавляем сообщение в контекст базы данных и сохраняем изменения
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Create),"Messages"); // Перенаправляем на страницу создания сообщения снова                
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден"); // Если пользователь не найден, добавляем ошибку в ModelState
                }
            }
            return View(message); // Возвращаем представление с сообщением для исправления ошибок
        }        
    }
}
