using HomeSync.Data;
using HomeSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HomeSync.Controllers
{
	public class RoomController : Controller
	{
		private readonly DbContextApp _context;

		public RoomController(DbContextApp context)
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
			try
			{
				var result = _context.Room.FromSqlRaw("EXEC ViewRooms {0}", HttpContext.Session.GetInt32("Id")).ToList();
				ViewData["USERID"] = HttpContext.Session.GetInt32("Id");
				ViewBag.res = result;

				return View("Index", result);
			}
			catch (Exception ex)
			{

				Console.WriteLine($"An error occurred: {ex.Message}");
				ViewBag.res = new List<Assigned_to>();
			}

			return View();
		}
		
		public IActionResult Index1()
		{
			try
			{
				var result = _context.Room.FromSqlRaw("select * from Room").ToList();
				ViewBag.res = result;
				return View("Index1", result);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				ViewBag.res = new List<Assigned_to>();
			}

			return View("Index1");
		}
		[HttpPost]
		public IActionResult SetRoom1(int RoomId)
		{
			try
			{
				IEnumerable<Room> rooms = _context.Room.FromSqlRaw($"SELECT * FROM Room WHERE room_id = {RoomId}").ToList();
				if (!rooms.IsNullOrEmpty())
				{
					_context.Database.ExecuteSqlRaw("EXEC AssignRoom {0} , {1}", HttpContext.Session.GetInt32("Id"), RoomId);
					TempData["AlertMessage"] = "Room Assigned Successfully.";
				}
				else
				{
					TempData["AlertMessage"] = "Room Not Found.";
				}
				return Index1();
			}
			catch (Exception ex)
			{

				Console.WriteLine($"An error occurred: {ex.Message}");
				ViewBag.res = new List<Assigned_to>();
			}

			return Index1();
		}

		public IActionResult Index2()
		{
			return View("Index2");
		}
		public IActionResult Index3()
		{
			return View("Index3");
		}

		[HttpPost]
		public IActionResult Schedule(int RoomId, DateTime start, DateTime end, string action)
		{
			IEnumerable<Room> rooms = _context.Room.FromSqlRaw($"SELECT * FROM Room WHERE room_id = {RoomId}").ToList();
			/*int? Id = HttpContext.Session.GetInt32("SessionUserId");
            if (Id == null || Id.Value == -1)
            {
                return RedirectToAction("Login_Register", "Login_Register");
            }*/
			if (!rooms.IsNullOrEmpty())
			{
				int res = _context.Database.ExecuteSqlInterpolated($"CreateSchedule @creator_id={HttpContext.Session.GetInt32("Id")}, @room_id={RoomId}, @start_time={start}, @end_time={end}, @action={action}");
				if (res > 0)
				{
					TempData["AlertMessage"] = "Schedule Created Successfully.";
				}
				else
				{
					TempData["AlertMessage"] = "Schedule Creation Failed.";
				}

			}
			else
			{
				TempData["AlertMessage"] = "Room Not Found.";
			}
			return View("Index2");
		}

		[HttpPost]
		public IActionResult SetStatus(int RoomId, string status)
		{
			IEnumerable<Room> rooms = _context.Room.FromSqlRaw($"SELECT * FROM Room WHERE room_id = {RoomId}").ToList();

			if (!rooms.IsNullOrEmpty())
			{
				_context.Database.ExecuteSqlRaw("EXEC RoomAvailability {0} , {1}", RoomId, status);
				TempData["AlertMessage"] = "Room Status Changed Successfully.";
			}
			else
			{
				TempData["AlertMessage"] = "Room Not Found.";
			}

			return Index3();
		}

		public IActionResult Index4()
		{
			try
			{
				var result = _context.Room.FromSqlRaw(" Exec ViewRoom").ToList();
				ViewBag.res = result;
				return View("Index4");
			}
			catch (Exception ex)
			{

				Console.WriteLine($"An error occurred: {ex.Message}");
				ViewBag.res = new List<Assigned_to>();
			}
			return View("Index4");
		}

		public Boolean IsRoom(int RoomId)
		{
			try
			{
				var result = _context.Room.FromSqlRaw("select * from Room where room_id={0}", RoomId).ToList();
				if (result.Count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine($"An error occurred: {ex.Message}");
				ViewBag.res = new List<Assigned_to>();
			}
			return false;
		}


	}
}
