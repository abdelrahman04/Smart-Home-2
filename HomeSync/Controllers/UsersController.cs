using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSync.Models;
using HomeSync.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace HomeSync.Controllers
{
    public class UsersController : Controller
    {
        private readonly DbContextApp _context;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(DbContextApp context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				TempData["AlertMessage"] = "Please Login First.";
				return RedirectToAction("Index", "Home");
			}

			List<Users> users = _context.Users.ToList();
            return View(users);
        }
		public IActionResult LogIn(string email, string password)
		{
			var em = new SqlParameter("@email", email);
			var pass = new SqlParameter("@password", password);
            
            
            try
            {
                var outputParameter1 = -1;
                var u = _context.Iddd.FromSqlRaw("EXEC UserLogin {0},{1}", em, pass).ToList()[0];
                Users a = _context.Users.FromSqlRaw("EXEC ViewProfile {0}",u.Id).ToList()[0];
				TempData["AlertMessage"] = "Log in Successful.";
                outputParameter1 = u.Id;
                if (outputParameter1!=-1)
                {
                    HttpContext.Session.SetInt32("Id", Convert.ToInt32(outputParameter1));
                    //Console.WriteLine(outputParameter1.ToString());
                    //Console.WriteLine(HttpContext.Session.GetInt32("Id"));
                    HttpContext.Session.SetString("Type",a.Type);
                    ViewData["USERID"] = outputParameter1;
                    ViewData["Type"] = a.Type;

                }
                else
                {
                    TempData["AlertMessage"] = "Invalid Credintials.";
                    return RedirectToAction("Index", "Home");
                }
                
                return RedirectToAction("Index");
			}
			catch (Exception ex)
           {
                TempData["AlertMessage"] = "Invalid Credintials.";
                return RedirectToAction("Index","Home");
            }
            
        }
		public IActionResult Register(string email,string fname,string lname,DateTime birth,string pass)
		{
			try
			{
				var o = -1;

				var v = _context.Iddd.FromSqlRaw("EXEC UserRegister {0},{1},{2},{3},{4},{5}", "Admin", email, fname, lname, birth, pass).ToList()[0];
				o = v.Id;
				TempData["AlertMessage"] = "Successful Register.";
					HttpContext.Session.SetInt32("Id", o);
					//Console.WriteLine(outputParameter1.ToString());
					//Console.WriteLine(HttpContext.Session.GetInt32("Id"));
					HttpContext.Session.SetString("Type","Admin");
                ViewData["USERID"] = o;
                ViewData["Type"] = "Admin";

                return RedirectToAction("Index");
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				TempData["AlertMessage"] = "Email Exists In Another Account.";
				return RedirectToAction("Register", "Home");
			}
		}
		[HttpGet]
		public IActionResult ViewProfile()
		{
			try
			{
				var result = _context.Users.FromSqlRaw("EXEC ViewProfile {0}", HttpContext.Session.GetInt32("Id")).ToList();
				ViewBag.res = result;

				foreach (var item in result)
				{
					Console.WriteLine();
					Console.WriteLine(item.Age);
					Console.WriteLine(item.BirthDate);
					Console.WriteLine(item.Email);
					Console.WriteLine();
				}

				return View("Index", result);
			}
			catch (Exception ex)
			{
				ViewBag.res = new List<Assigned_to>();
				Console.WriteLine("No user Logged in");

				TempData["AlertMessage"] = "No user Logged in";

				return RedirectToAction("Index", "Home");
			}

		}
		[HttpGet]
		public IActionResult ViewGuests()
		{
			try
			{
				var result = _context.Users.FromSqlRaw("select * from users where id in (select u.id from Users u inner join Guest g on u.id = g.guest_id where g.guest_of= {0});", HttpContext.Session.GetInt32("Id")).ToList();
				ViewBag.res = result;

				foreach (var item in result)
				{
					Console.WriteLine();
					Console.WriteLine(item.Age);
					Console.WriteLine(item.BirthDate);
					Console.WriteLine(item.Email);
					Console.WriteLine();
				}

				return View("Index1", result);
			}
			catch (Exception ex)
			{
				ViewBag.res = new List<Assigned_to>();
				Console.WriteLine("No user Logged in");

				TempData["AlertMessage"] = "No user Logged in";

				return RedirectToAction("Index", "Home");
			}

		}
		public IActionResult Index1()
		{
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				TempData["AlertMessage"] = "Please Login First.";
				return RedirectToAction("Index", "Home");
			}
			try
			{
				var result = _context.tri.FromSqlRaw("EXEC GuestNumber {0}", HttpContext.Session.GetInt32("Id")).ToList()[0].Id;
				ViewData["GuestCount"] = result;
				Console.WriteLine(result);
				Console.WriteLine(HttpContext.Session.GetInt32("Id"));
				return View("Index1");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				ViewBag.res = new List<Assigned_to>();
			}
			return View("Index1");
		}
		[HttpPost]
		public IActionResult RemoveGuest(int GuestID)
		{
			try
			{
				Console.WriteLine("fwf");
				Console.WriteLine(GuestID);
				Console.WriteLine();

				

				_context.Database.ExecuteSqlRaw("EXEC GuestRemove {0},{1}", GuestID, HttpContext.Session.GetInt32("Id"));
				TempData["AlertMessage"] = "Guest Removed Successfully.";
			}
			catch (Exception ex)
			{

				TempData["AlertMessage"] = "Invalid Guest ID";

			}
			return Index1();
		}

		[HttpPost]
		public IActionResult GuestCount(int count)
		{
			try
			{
				_context.Database.ExecuteSqlRaw("EXEC GuestsAllowed {0},{1}", HttpContext.Session.GetInt32("Id"), count);
				TempData["AlertMessage"] = "count updated.";
			}
			catch (Exception ex)
			{

				TempData["AlertMessage"] = "Invalid count";

			}
			return Index1();
		}

	}
}
