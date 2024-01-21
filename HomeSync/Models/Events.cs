using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
    public class Events
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("event_id")]
        public int EventId { get; set; }

		[Column("user_assigned_to")]
		public int? User_assigned_to { get; set; }

		[ForeignKey("User_assigned_to")]
        public Users? Users { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("reminder_date")]
        public DateTime? ReminderDate { get; set; }



    }
}
