using Microsoft.AspNetCore.Mvc;
using HomeSync.Models;
using HomeSync.Data;

namespace HomeSync.Controllers
{
	public class Assigned_toController: Controller
	{
		private readonly DbContextApp _context;

		public Assigned_toController(DbContextApp context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			List<Assigned_to> assigned_Tos = _context.Assigned_to.ToList();
			return View(assigned_Tos);
		}
		
	}
}
