using Microsoft.AspNetCore.Mvc;
using HomeSync.Data;
using HomeSync.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HomeSync.Controllers
{
    public class DeviceController : Controller
    {
        private readonly DbContextApp _context;

        public DeviceController(DbContextApp context)
        {
            _context = context;
        }
		/*View device charge*/
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
			/*	var v = _context.ViewCharge.FromSqlRaw("EXEC ViewMyDeviceCharge {0}, {1}, {2}", 10, charge, location).ToList()[0];*/
			var v = _context.ViewCharge.FromSqlRaw("EXEC ViewMyDeviceCharge {0}, {1}", device_id, charge).ToList()[0];
			Console.WriteLine();
			Console.WriteLine(v.Charge);
			Console.WriteLine();
			ViewData["Charge"] = v.Charge;
			return View("Index");
        }
		/* Add a new device*/

		/*    public IActionResult Index(int device_id, string status, int battery, int location, string type)
			{
				  _context.Database.ExecuteSqlRaw("EXEC AddDevice @device_id, @status, @battery, @location, @type",
					  new SqlParameter("@device_id", device_id),
					  new SqlParameter("@status", status),
					  new SqlParameter("@battery", battery),
					  new SqlParameter("@location", location),
					  new SqlParameter("@type", type));
				_context.ExecuteAddDevice(device_id, status, battery, location, type);
				return RedirectToAction("Index");
			}*/
		[HttpPost]
		public IActionResult ExecuteAddDevice(int device_id, string status, int battery, int location, string type)
		{

			var deviceId = new SqlParameter("@device_id", device_id);
			var theStatus = new SqlParameter("@status", status);
			var batteryStatus = new SqlParameter("@battery", battery);
			var room = new SqlParameter("@location", location);
			var Type = new SqlParameter("@type", type);
			Console.WriteLine();
			Console.WriteLine(device_id + " " + status + " " + battery + " " + location + " " + type);
			Console.WriteLine();
			_context.Database.ExecuteSqlRaw("EXEC dbo.AddDevice {0}, {1}, {2}, {3}, {4}", deviceId, theStatus, batteryStatus, room, Type);
			_context.SaveChanges();
			TempData["AlertMessage"] = "Device Added Successfully!";

			return RedirectToAction("Index");
		}
		/*Find the locations of all devices out of battery*/
		
        /*Set the status of all devices out of battery to charging*/
        [HttpPost]
        public IActionResult Charging()
        {
            _context.Database.ExecuteSqlRaw("EXEC Charging");
			TempData["AlertMessage"] = "Operation Successful!";

			return View("Index");
        }
		/*Get the location where more than two devices have a dead battery*/
		[HttpGet]
		public IActionResult OutOfBattery()
		{
			Console.WriteLine("Ay batee5");
			try
			{
				var rooms = _context.Device.FromSqlRaw("EXEC OutOfBattery").ToList();
				Console.WriteLine("Ay batee52");
				for (int i = 0; i < rooms.Count; i++)
					Console.Write(rooms[i]);
				Console.WriteLine();
				ViewBag.rooms = rooms;
				
				return View("Index", rooms);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error has occurred: {ex.Message}");
				return View("Index");
			}
		}
		[HttpGet]
        public IActionResult NeedCharge()
        {
            Console.WriteLine("shwayat batee5");
            try
            {
                var rooms = _context.Device.FromSqlRaw("EXEC NeedCharge").ToList();
                ViewBag.de = rooms;
                Console.WriteLine("the batee5");
                foreach(var room in ViewBag.de)
                {
                    Console.WriteLine(room.Type + " " + room.Status);
                }
				

				return View("Index", rooms);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred: {ex.Message}");
                return View("Index");
            }
        }
		public IActionResult Index()
		{
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				TempData["AlertMessage"] = "Please Login First.";
				return RedirectToAction("Index", "Home");
			}
			List<Device> devices = _context.Device.ToList();
			return View(devices);


		}
		/* if(HttpContext.Session.GetInt32("Id") == null)
             {
                 TempData["AlertMessage"] = "Please Login First.";
                 return RedirectToAction("Index", "Home");
             }*/
	}
}
