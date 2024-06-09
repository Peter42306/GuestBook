using GuestBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuestBook.Controllers
{
    public class MessagesController : Controller
    {
        private readonly GuestBookContext _context;

        public MessagesController(GuestBookContext context)
        {
            _context = context;
        }

        //private readonly GuestBookContext _context;

        //public UsersController(GuestBookContext context)
        //{
        //    _context = context;
        //}
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Name"] = HttpContext.Session.GetString("Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create([Bind("MessageContent")] Message message)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                var user = _context.Users.SingleOrDefault(u => u.Name == userName);

                if (user != null)
                {
                    message.UserId = user.Id;
                    message.MessageDate= DateTime.Now;

                    _context.Messages.Add(message);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index),"GuestBook");                
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                }
            }
            return View(message);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("MessageContent")] Message message)
        //{

        //}


        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
