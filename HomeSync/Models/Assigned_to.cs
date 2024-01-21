
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
	public class Assigned_to
	{

		/*
		 * admin_id INT FOREIGN KEY REFERENCES [Admin],
task_id INT FOREIGN KEY REFERENCES Task,
[user_id] INT FOREIGN KEY REFERENCES Users,
CONSTRAINT Assignement PRIMARY KEY (admin_id, task_id, [user_id])

		*
		*/
		[Column("admin_id")]
		public int AdminId { get; set; }
		[Column("task_id")]
		public int TaskId { get; set; }
		[Column("user_id")]
		public int UsersId { get; set; }
		public Admin Admin { get; set; }
		public Task Task { get; set; }
		public Users Users { get; set; }
	}
}
