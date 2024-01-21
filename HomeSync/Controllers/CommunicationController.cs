using HomeSync.Data;
using HomeSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.IdentityModel.Tokens;

namespace HomeSync.Controllers
{
    public class CommunicationController : Controller
    {
        private readonly DbContextApp _context;
        public CommunicationController(DbContextApp context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				TempData["AlertMessage"] = "Please Login First.";
				return RedirectToAction("Index", "Home");
			}
			List<Communication> communication = _context.Communication.ToList();
			ViewBag.IsConditionMet = true;
			return View(communication);
        }
        [HttpPost]
        public IActionResult Index(int senderId, int receiverId, string title, string content, TimeOnly timeSent, TimeOnly timeReceived)
        {
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				TempData["AlertMessage"] = "Please Login First.";
				return RedirectToAction("Index", "Home");
			}
			try
            {
                _context.ExecuteSendMessage((int)HttpContext.Session.GetInt32("Id"), receiverId, title, content, timeSent, timeReceived);
                TempData["AlertMessage"] = "Message Sent Succesfully.";
				return RedirectToAction("Index");
			}catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["AlertMessage"] = "Message Failed To Send, No Such User.";
                return View("Index", _context.Communication);
            }
			
        }
        [HttpGet]
        public IActionResult Filtered(int senderId)
        {
                try
                {
                    var res = _context.ExecuteShowMessages((int)HttpContext.Session.GetInt32("Id"), senderId);
                    ViewBag.IsConditionMet = true;
                    TempData["AlertMessage"] = "Succesful Request.";
                    return View("Index", res);
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = "No Messages.";
                    return View("Index",_context.Communication);
                }
            }
        [HttpPost]
        public IActionResult DeleteLast()
        {
            try
            {
                if (!_context.Communication.IsNullOrEmpty())
                {
                    if (!HttpContext.Session.GetString("Type").Equals("Admin"))
                    {
                        TempData["AlertMessage"] = "You Need To Be An Admin.";
                    }
                    else
                    {
                        _context.Database.ExecuteSqlRaw("EXEC dbo.DeleteMsg");
                        _context.SaveChanges();
                        TempData["AlertMessage"] = "Message Deleted Succesfully.";
                    }
                }else
                {
					TempData["AlertMessage"] = "No Messages To Delete.";
				}
                return RedirectToAction("Index");
            }
            catch (Exception e) { }
			return View("Index",_context.Communication.ToList());
		}
    }
}
