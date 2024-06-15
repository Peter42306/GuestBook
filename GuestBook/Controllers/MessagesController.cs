using GuestBook.Models;
using GuestBook.Services;
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
        MyLoggerTXT _myLoggerTxt;
        MyLoggerXlsx _myLoggerXlsx;

        public MessagesController(GuestBookContext context, MyLoggerTXT myLoggerTxt, MyLoggerXlsx myLoggerXlsx)
        {
            _context = context;
            _myLoggerTxt = myLoggerTxt;
            _myLoggerXlsx = myLoggerXlsx;
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

                    _myLoggerTxt.Log($"Пользователь {userName} написал сообщение {message.MessageContent}");
                    _myLoggerXlsx.Log($"Пользователь {userName} написал сообщение {message.MessageContent}");

                    return RedirectToAction(nameof(Create),"Messages"); // Перенаправляем на страницу создания сообщения снова                
                }
                else
                {
                    _myLoggerTxt.Log($"Пользователь {userName} не написал  сообщение так не найден в БД");
                    _myLoggerXlsx.Log($"Пользователь {userName} не написал  сообщение так не найден в БД");

                    ModelState.AddModelError("", "Пользователь не найден"); // Если пользователь не найден, добавляем ошибку в ModelState
                }                
            }

            _myLoggerTxt.Log($"Пользователь {User.Identity.Name} не удалось отправить сообщение");
            _myLoggerXlsx.Log($"Пользователь {User.Identity.Name} не удалось отправить сообщение");

            return View(message); // Возвращаем представление с сообщением для исправления ошибок
        }        
    }
}
