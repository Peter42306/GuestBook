using GuestBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    public class UsersController : Controller
    {
        private readonly GuestBookContext _context;

        public UsersController(GuestBookContext context)
        {
            _context = context;
        }


        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login(User login)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        CookieOptions cookieOptions = new CookieOptions();
        //        cookieOptions.Expires = DateTime.Now.AddDays(10);
        //        Response.Cookies.Append("login", login.Name, cookieOptions);
        //        return RedirectToAction();
        //    }
        //    return View(login);
        //}










        //private readonly GuestBookContext _context;

        //public GuestBookController(GuestBookContext context)
        //{
        //	_context = context;
        //}




        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
