using Microsoft.AspNetCore.Mvc;
using HomeSync.Data;
using HomeSync.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HomeSync.Controllers
{
	public class ViewChargeController : Controller
	{
		private readonly DbContextApp _context;
		public ViewChargeController(DbContextApp context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult ViewCharge(int device_id)
		{
			/* var entity = _context.Device.Find(device_id);
             if (entity == null)
             {
                 return View("Index", "NotFound");
             }
             else
             {
                 int battery = entity.BatteryStatus.Value;
                 return View(battery);
             }*/
			int u = -1;
			int charge = 0;
			int location = 0;
			var v = _context.ViewCharge.FromSqlRaw("EXEC ViewMyDeviceCharge {0}, {1}, {2}", device_id, charge, location).ToList()[0];
			ViewBag.charge = v;
			return View("Index1", v);

		}
		public IActionResult Index()
		{
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				TempData["AlertMessage"] = "Please Login First.";
				return RedirectToAction("Index", "Home");
			}
			List<ViewCharge> charge = _context.ViewCharge.ToList();
			return View(charge);


		}
	}
}
