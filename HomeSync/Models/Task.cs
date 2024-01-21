    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Task_id")]
        public int TaskId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("creation_date")]
        public DateTime? CreationDate { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("creator")]
        public int? Creator {  get; set; }

        [ForeignKey("Creator")]
        public Admin Admin { get; set; }


        [Column("status")]
        public string? Status { get; set; }

        [Column("reminder_date")]
        public DateTime? ReminderDate { get; set; }

        public int? Priority { get; set; }


    }
}
