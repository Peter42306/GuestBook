using GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using GuestBook.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using GuestBook.Services;

namespace GuestBook.Controllers
{    
    /// <summary>
    /// Контроллер отвечает за управление учетными записями пользователей              
    /// _context содержит ссылку на контекст базы данных
    /// Конструктор принимает контекст базы данных GuestBookContext для взаимодействия с базой данных
    /// </summary>
    public class AccountController : Controller
    {
        private readonly GuestBookContext _context;        

        private readonly ISaltGenerator _saltGenerator;
        private readonly IPasswordHasher _passwordHasher;

        MyLoggerTXT _myLoggerTxt;
        MyLoggerXlsx _myLoggerXlsx;
               

        public AccountController(GuestBookContext context, ISaltGenerator saltGenerator, IPasswordHasher passwordHasher, MyLoggerTXT myLoggerTxt, MyLoggerXlsx myLoggerXlsx)
        {
            _context = context;
            _saltGenerator = saltGenerator;
            _passwordHasher = passwordHasher;
            _myLoggerTxt = myLoggerTxt;
            _myLoggerXlsx = myLoggerXlsx;
        }

        //==============================================================
        // GET: Account/Login        
        /// <summary>
        /// Отображает страницу входа пользователя
        /// </summary>
        /// <returns>Представление страницы входа пользователя</returns>
        public IActionResult Login()
        {
            var cookieValue = Request.Cookies["login"]; // Проверка наличия куки

            if (!string.IsNullOrEmpty(cookieValue))
            {
                _myLoggerTxt.Log($"Пользователь {cookieValue} вошёл в систему (cookie value)");
                _myLoggerXlsx.Log($"Пользователь {cookieValue} вошёл в систему (cookie value)");

                HttpContext.Session.SetString("Name", cookieValue);// Сохранение имени пользователя в сессии

                return RedirectToAction("Create", "Messages");// Пользователь уже залогинен, перенаправляем на страницу создания сообщений
            }

            return View(); // Представление страницы входа пользователя
        }

        // POST: Account/Login        
        /// <summary>
        /// Обрабатывает запрос на вход пользователя
        /// </summary>
        /// <param name="loginModel">Модель данных для входа</param>
        /// <returns>Редирект на страницу создания сообщений в случае успешного входа, иначе возвращает представление страницы входа с сообщениями об ошибках</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // Проверяется, есть ли вообще пользователи в базе данных.
                if (!_context.Users.Any())
                {
                    ModelState.AddModelError("", "В базе данных ещё нет пользователей");
                    return View(loginModel);
                }

                var user = _context.Users.SingleOrDefault(u => u.Name == loginModel.Name);// Поиск пользователя с указанным логином. Если такой пользователь не найден, выводится сообщение об ошибке

                if (user == null)
                {
                    ModelState.AddModelError("", "В базе данных такой пользователь не найден");

                    _myLoggerTxt.Log($"Незарегистрированный пользователь {loginModel.Name} пытался войти в вистему");
                    _myLoggerXlsx.Log($"Незарегистрированный пользователь {loginModel.Name} пытался войти в вистему");

                    return View(loginModel);
                }


                // Проверка пароля
                if (user.Password != _passwordHasher.HashPassword(loginModel.Password,user.Salt))
                {
                    ModelState.AddModelError("", "Неверный пароль");

                    _myLoggerTxt.Log($"Пользователь {loginModel.Name} ввёл неверный пароль");
                    _myLoggerXlsx.Log($"Пользователь {loginModel.Name} ввёл неверный пароль");
                    return View(loginModel);
                }


                //// Проверка пароля
                //if (user.Password != Utilities.HashPassword(loginModel.Password, user.Salt))
                //{
                //    ModelState.AddModelError("", "Неверный пароль");
                //    return View(loginModel);
                //}


                // Создание Claims, (утверждение) представляет единичную информацию о пользователе (например, его имя)
                // Создание списка утверждений, создается новый список List<Claim>, который будет содержать утверждения для текущего пользователя
                // Добавление утверждения. В список добавляется новое утверждение (Claim), где тип утверждения (ClaimTypes.Name) указывает, что это имя пользователя, а user.Name содержит значение имени
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Name)
                };

                // Создание ClaimsIdentity
                // Назначение: ClaimsIdentity представляет собой идентичность пользователя, которая может содержать одно или несколько утверждений
                // Передача списка утверждений: Здесь создается объект ClaimsIdentity, принимающий список утверждений(claims), созданный ранее
                // Указание схемы аутентификации: Второй параметр указывает схему аутентификации, используемую для этих утверждений. В данном случае используется схема аутентификации куки - файлов(CookieAuthenticationDefaults.AuthenticationScheme)
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // аутентифиция пользователя, создавая для него сессию с использованием куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Создание и установка куки при успешной аутентификации
                CookieOptions cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddDays(1) };
                Response.Cookies.Append("login", user.Name, cookieOptions);

                HttpContext.Session.SetString("Name", user.Name); // Сохранение информации о пользователе в сессии

                _myLoggerTxt.Log($"Пользователь {user.Name} вошёл в систему (log in)");
                _myLoggerXlsx.Log($"Пользователь {user.Name} вошёл в систему (log in)");

                return RedirectToAction("Index", "GuestBook");// перенаправляет пользователя на домашнюю страницу
            }

            return View(loginModel);// представление Login будет возвращено вместе с моделью loginModel, что позволяет пользователю видеть введенные данные и сообщения об ошибках
        }

        //==============================================================
        // GET: Account/Register
        // для отображения страницы регистрации
        /// <summary>
        /// Отображает страницу регистрации нового пользователя
        /// </summary>
        /// <returns>Представление страницы регистрации</returns>
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        // Обрабатывает запрос на регистрацию нового пользователя
        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        /// <param name="registerModel">Модель данных для регистрации</param>
        /// <returns>При успешной регистрации перенаправляет на страницу входа, иначе возвращает представление с моделью данных для исправления ошибок</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel registerModel)
        {
            // Проверка на существование пользователя с таким же именем в базе данных
            if (_context.Users.Any(u => u.Name == registerModel.Name))
            {
                _myLoggerTxt.Log($"Пользователь {registerModel.Name} попытался повторно зарегистрироваться");
                _myLoggerXlsx.Log($"Пользователь {registerModel.Name} попытался повторно зарегистрироваться");

                ModelState.AddModelError("Name", "Пользователь с таким именем уже существует в базе данных");
            }

            // Если модель проходит валидацию, сохраняем пользователя в базе данных
            if (ModelState.IsValid)
            {
                User user = new User(); // Создаем новый объект пользователя
                user.Name = registerModel.Name; // Генерируем случайную строку (соль) для шифрования пароля

                // соль и хеширование с помощью статических методов 
                //string salt = Utilities.GenerateSalt(); // Генерируем случайную строку (соль) для шифрования пароля
                //user.Password = Utilities.HashPassword(registerModel.Password, salt); // // Хешируем пароль из модели регистрации, используя соль, и устанавливаем хешированный пароль в объект пользователя
                //user.Salt = salt; // Устанавливаем соль в объект пользователя

                // соль и хеширование с помощью сервисов 
                string salt =_saltGenerator.GenerateSalt();
                user.Password = _passwordHasher.HashPassword(registerModel.Password, salt);
                user.Salt= salt;

                _context.Users.Add(user); // Добавляем пользователя в контекст базы данных
                _context.SaveChanges(); // Сохраняем изменения в базе данных
                                
                //CookieOptions cookieOptions = new CookieOptions(); // При успешной регистрации устанавливаем куки для аутентификации пользователя
                //cookieOptions.Expires = DateTime.Now.AddDays(1); // Устанавливаем срок действия куки на 1 день
                //Response.Cookies.Append("login", registerModel.Name, cookieOptions); // Добавляем куки в ответ с именем пользователя, чтобы позже использовать его для аутентификации

                _myLoggerTxt.Log($"Пользователь {registerModel.Name} успешно прошёл регистрацию)");
                _myLoggerXlsx.Log($"Пользователь {registerModel.Name} успешно прошёл регистрацию)");

                return RedirectToAction("Index", "GuestBook"); // Перенаправляем пользователя на страницу входа после успешной регистрации
            }

            _myLoggerTxt.Log($"Пользователь {registerModel.Name} не прошёл валидацию формы при регистрации)");
            _myLoggerXlsx.Log($"Пользователь {registerModel.Name} не прошёл валидацию формы при регистрации)");

            return View(registerModel); // Возвращаем представление с моделью регистрации для отображения ошибок валидации
        }

        //==============================================================
        /// <summary>
        /// Обрабатывает запрос на выход пользователя из системы
        /// Удаляет куки, связанные с аутентификацией, и очищает сессию
        /// </summary>
        /// <returns>Перенаправляет на главную страницу гостевой книги после успешного выхода</returns>
        public ActionResult Logout()
        {
            var userName = HttpContext.Session.GetString("Name"); // Получаем имя пользователя из сессии

            Response.Cookies.Delete("login"); // Удаляем куки, связанные с аутентификацией
                       

            HttpContext.Session.Clear(); // Очищаем текущую сессию

            _myLoggerTxt.Log($"Пользователь {userName} вышел из системы (logout)");
            _myLoggerXlsx.Log($"Пользователь {userName} вышел из системы (logout)");

            return RedirectToAction("Index", "GuestBook"); // Перенаправляем пользователя на главную страницу гостевой книги после успешного выхода
        }
    }
}
