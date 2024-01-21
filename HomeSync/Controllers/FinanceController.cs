using Microsoft.AspNetCore.Mvc;
using HomeSync.Models;
using HomeSync.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace HomeSync.Controllers
{
    public class FinanceController : Controller {


        private readonly DbContextApp _context;
        public FinanceController(DbContextApp context)
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
			List<Finance> finances = _context.Finance.ToList();
            return View(finances);
		}
		[HttpPost]
        public IActionResult Index(decimal Amount,string Status,DateTime Date) {
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				TempData["AlertMessage"] = "Please Login First.";
				return RedirectToAction("Index", "Home");
			}
			if (ModelState.IsValid)
            {
                try
                {
                    _context.ExecuteReceiveMoney((int)HttpContext.Session.GetInt32("Id"), "incoming", Amount, Status, Date);
                    TempData["AlertMessage"] = "Money Received Succesfully.";
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = "The User You Are Trying To Receive From Doesn't Exist On The Site.";
                }
				return RedirectToAction("Index");
            }
			TempData["AlertMessage"] = "Transaction Failed.";
			return View();
        }
        public IActionResult Index1(int senderId,int receiverId, decimal amount, string status, DateTime deadline)
        {
			if (ModelState.IsValid)
			{
                try
                {  
                    _context.ExecutePlanPayment((int)HttpContext.Session.GetInt32("Id"), receiverId, amount, status, deadline);
				    TempData["AlertMessage"] = "Transaction Planned Succesfully. ";
                   
				}
				catch (Exception ex) {
					TempData["AlertMessage"] = "The Receiver Doesn't Exist.";
				}
				return RedirectToAction("Index");
				//return View(result);
			}
			return View();
		}
        }
    }
