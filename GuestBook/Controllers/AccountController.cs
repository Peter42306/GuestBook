using GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using GuestBook.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace GuestBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly GuestBookContext _context;

        public AccountController(GuestBookContext context)
        {
            _context = context;
        }

        //==============================================================

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // Проверяется, есть ли вообще пользователи в базе данных.
                if (_context.Users.ToList().Count==0)
                {
                    ModelState.AddModelError("", "В базе данных ещё нет пользователей");
                    return View(loginModel);
                }

                // Поиск пользователя с указанным логином. Если такой пользователь не найден, выводится сообщение об ошибке
                var users = _context.Users.Where(u => u.Name == loginModel.Name);
                if (users.ToList().Count==0)
                {
                    ModelState.AddModelError("", "В базе данных такой пользователь не найден");
                }

                // Получается первый(и, предположительно, единственный) пользователь с данным логином
                var user=users.First();
                                

                // Проверка пароля
                if (user.Password != Utilities.HashPassword(loginModel.Password,user.Salt))
                {
                    ModelState.AddModelError("", "Неверный пароль");
                    return View(loginModel);
                }


                // Создание Claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Name)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));



                //// Проверка пароля
                //if (user.Password!=hash.ToString())
                //{
                //    ModelState.AddModelError("", "Неверный пароль");
                //    return View(loginModel);
                //}

                // Сохранение информации о пользователе в сессии
                HttpContext.Session.SetString("Name", user.Name);
                return RedirectToAction("Create", "Messages");
            }
            return View(loginModel);
        }

        //==============================================================

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel registerModel)
        {
            // Проверка на существование пользователя с таким же именем в базе данных
            if (_context.Users.Any(u=>u.Name==registerModel.Name))
            {
                ModelState.AddModelError("Name", "Пользователь с таким именем уже существует в базе данных");
            }

            // Если модель проходит валидацию, сохраняем пользователя в базе данных
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Name = registerModel.Name;

                string salt = Utilities.GenerateSalt();
                user.Password = Utilities.HashPassword(registerModel.Password, salt);
                user.Salt = salt;

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login", "Account");
            }
            return View(registerModel);
        }        

        //==============================================================

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "GuestBook");
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(User login)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        CookieOptions cookieOptions = new CookieOptions();
        //        cookieOptions.Expires = DateTime.Now.AddMinutes(1);
        //        //cookieOptions.Expires = DateTime.Now.AddDays(10);
        //        Response.Cookies.Append("login", login.Name, cookieOptions);
        //        return RedirectToAction();
        //    }
        //    return View(login);
        //}

        //==============================================================





        //==============================================================
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
