using HomeSync.Data;
using HomeSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HomeSync.Controllers
{

    public class EventsController : Controller
    {
        private readonly DbContextApp _context;
        public EventsController(DbContextApp context)
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
            List<Events> events = _context.Calendar.ToList();
            return View(events);
        }

        /* Name: AssignUser.
          Assign  user  to  attend  an  event*/
        [HttpPost]
        public IActionResult AssignUser(int user_id, int event_id)
        {
            _context.Database.ExecuteSqlRaw("EXEC AssignUser @user_id=@userpara, @event_id=@eventpara",
                new SqlParameter("@userpara", user_id), new SqlParameter("@eventpara", event_id));
            return RedirectToAction("Index");
        }

        /* Name: Uninvited.
        Uninvite  a  specific  user  to  an  event */
        [HttpPost]
        public IActionResult Uninvited(int user_id, int event_id)
        {
            _context.Database.ExecuteSqlRaw("EXEC Uninvited @user_id=@userparam, @event_id=@eventparam",
                new SqlParameter("@userparam", user_id), new SqlParameter("@eventparam", event_id));
            return RedirectToAction("Index");
        }

        /* 
Name: RemoveEvent.
Remove an event from the system. */
        [HttpPost]
        public IActionResult RemoveEvent (int user_id, int event_id)
        {
            if (!HttpContext.Session.GetString("Type").Equals("Admin"))
            {
                TempData["AlertMessage"] = "You Need To Be An Admin.";
                return RedirectToAction("Index");
            }
            else
            {
                _context.Database.ExecuteSqlRaw("EXEC RemoveEvent @event_id=@eventparam,@user_id=@userparam",
              new SqlParameter("@eventparam", event_id), new SqlParameter("@userparam", user_id));
                return RedirectToAction("Index");
            }
        }

        /* Name: CreateEvent.
         Create  an  event  on  the  system */
        [HttpPost]
        public IActionResult CreateEvent(int event_id, int user_id, string name, string description, string location, DateTime reminder_date, int other_user_id)
        {
            _context.Database.ExecuteSqlRaw("EXEC CreateEvent @event_id=@event_idparam, @user_id=@user_idparam, @name=@nameparam, @description=@descriptionparam, @location=@locationparam, @reminder_date=@reminder_dateparam, @other_user_id=@other_user_idparam",
                new SqlParameter("@event_idparam", event_id),
                new SqlParameter("@user_idparam", user_id),
                new SqlParameter("@nameparam", name),
                new SqlParameter("@descriptionparam", description),
                new SqlParameter("@locationparam", location),
                new SqlParameter("@reminder_dateparam", reminder_date),
                new SqlParameter("@other_user_idparam", other_user_id));

            return RedirectToAction("Index");
        }

		/*
Name: ViewEvent.
View  an  event  given  the  event  name  and  creator 
if  the  event  doesn’t  exist  then  view  all  events  that belong  to  the  user  orderd  by  their  date.*/
		[HttpGet]
		public IActionResult ViewEvent(int user_id, int? event_id)
		{
			string sqlQuery = "EXEC ViewEvent @user_id=@userparam";
			var events = _context.Calendar.FromSqlRaw(sqlQuery, new SqlParameter("@userparam", user_id)).ToList();
			if (event_id.HasValue)
			{
				sqlQuery += ", @event_id=@eventparam";
				 events = _context.Calendar.FromSqlRaw(sqlQuery, new SqlParameter("@userparam", user_id), new SqlParameter("@eventparam", event_id)).ToList();

			}
			ViewBag.res = events;
			return View("Index", events);
		}
        
    }
}
