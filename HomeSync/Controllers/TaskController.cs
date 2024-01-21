using Microsoft.AspNetCore.Mvc;
using HomeSync.Data;

namespace HomeSync.Controllers
{
	using HomeSync.Models;
	using Microsoft.Data.SqlClient;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.VisualBasic;

	public class TaskController : Controller
	{
		private readonly DbContextApp _context;
		public readonly IHttpContextAccessor _httpContextAccessor;


		public TaskController(DbContextApp context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			ViewData["USERID"] = _httpContextAccessor.HttpContext.Session.GetInt32("Id");

		}



		/*
             Name: ViewMyTask.
            Input: @user_id int.
            Output: Table containing all the details of the tasks.

        */
		[HttpGet]
		public IActionResult ViewMyTask()
		{
			try
			{

				var result = _context.Task.FromSqlRaw("EXEC ViewMyTask @user_id={0}", HttpContext.Session.GetInt32("Id")).ToList();

				ViewBag.res = result;

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

		/*
         View task status given the @user_id and the @creator of the task. (The recently created reports
          should be shown first).
          Signature:
          Name: ViewTask.
          Input: @user_id int, @creator varchar(50).
          Output: Table containing all details of the task(s)
      */

		[HttpGet]
		public IActionResult ViewTask(int userId)
		{//"EXEC ViewTask @user_id={0}, @creator={1}", user_id, creator
		 //_context.Assigned_to.FromSqlRaw("").ToList();
		 //EXEC ViewTask @user_id={0}, @creator={1}

			try
			{

				var result = _context.Task.FromSqlRaw("EXEC ViewTask @user_id={0}, @creator={1}", userId, HttpContext.Session.GetInt32("Id")).ToList();

				ViewBag.guest = result;

				return View("Index", result);
			}
			catch (Exception ex)
			{

				Console.WriteLine($"An error occurred: {ex.Message}");
				ViewBag.res = new List<Assigned_to>();
			}

			return View();
		}

		/*
                * Finish their task.
       Signature:
       Name: FinishMyTask.
       Input: @user_id int, @title varchar(50).
       Output: Nothing.

        */
		[HttpPost]
		public IActionResult FinishMyTask(string title)
		{
			Console.WriteLine("Debug");
			_context.Database.ExecuteSqlRaw("Exec FinishMyTask " + HttpContext.Session.GetInt32("Id") + ",'" + title + "'");
			Console.WriteLine(HttpContext.Session.GetInt32("Id") + " " + title);
			TempData["AlertMessage"] = "Task Finished successfully!";
			return RedirectToAction("Index");
		}

		/*
             Add a reminder to a task.
                Signature:
                Name: AddReminder.
                Input: @task_id int, @reminder datetime.
                Output: Nothing.
        */
		[HttpPost]
		public IActionResult AddReminder(int task_id, DateTime reminder)
		{

			_context.Database.ExecuteSqlRaw("Exec AddReminder @task_id, @reminder",
				new SqlParameter("@task_id", task_id),
				new SqlParameter("@reminder", reminder));
			TempData["AlertMessage"] = "Reminder Added successfully!";

			Console.WriteLine();
			Console.WriteLine(reminder + " " + task_id);
			Console.WriteLine();

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult NewTask(string Name, DateTime DueDate, string Category, DateTime ReminderDate, int Priority, string Status, int userId)
		{
			Console.WriteLine();
			Console.WriteLine("New Task");
			Console.WriteLine();
			Console.WriteLine(Name + " " + DueDate + " " + Category + " " + ReminderDate + " " + Priority + " " + Status);
			Console.WriteLine();
			_context.Database.ExecuteSqlRaw("Exec AdminAddTask {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
				new SqlParameter("@user_id", userId),
				new SqlParameter("@creator", HttpContext.Session.GetInt32("Id")),
				new SqlParameter("@name", Name),
				new SqlParameter("@category", Category),
				new SqlParameter("@priority", Priority),
				new SqlParameter("@status", Status),
				new SqlParameter("@reminder", ReminderDate),
				new SqlParameter("@deadline", DueDate)

			);

			TempData["AlertMessage"] = "Task Added successfully!";



			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult UpdateTaskDeadline(DateTime deadline, int task_id)
		{
			_context.Database.ExecuteSqlRaw("EXEC UpdateTaskDeadline @deadline=@deadlineParam, @task_id=@taskIdParam",
				new SqlParameter("@deadlineParam", deadline),
				new SqlParameter("@taskIdParam", task_id));
			TempData["AlertMessage"] = "Deadline updated successfully!";
			Console.WriteLine();
			Console.WriteLine(deadline + " " + task_id);
			Console.WriteLine();
			return RedirectToAction("Index");
		}

		//Extra method if user unchecked the task
		[HttpPost]
		public IActionResult UnFinishMyTask(string title)
		{

			_context.Database.ExecuteSqlRaw("Exec UnFinishMyTask " + HttpContext.Session.GetInt32("Id") + ",'" + title + "'");
			Console.WriteLine(HttpContext.Session.GetInt32("Id") + " " + title);
			TempData["AlertMessage"] = "Task Back to Pending";

			return RedirectToAction("Index");
		}

		public IActionResult Index()
		{
			if (HttpContext.Session.GetInt32("Id") == null)
			{
				TempData["AlertMessage"] = "Please Login First.";
				return RedirectToAction("Index", "Home");
			}
			List<Task> tasks = _context.Task.ToList();
			return View(tasks);


		}
	}
}
