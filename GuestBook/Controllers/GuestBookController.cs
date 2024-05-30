using GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Controllers
{
	public class GuestBookController : Controller
	{
		private readonly GuestBookContext _context;

        public GuestBookController(GuestBookContext context)
        {
            _context = context;
        }
		

		// страница на которой можно смотреть все сообщения
		public async Task<IActionResult> Index()
		{
			var messageContext = _context.Messages.Include(m => m.User);			
			return View(messageContext);

			//return View(await _context.Messages.ToListAsync());
		}





		//public IActionResult Index()
		//{
		//	return View();
		//}
	}
}
