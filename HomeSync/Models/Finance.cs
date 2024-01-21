using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
    public class Finance
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("payment_id")]
        public int PaymentId { get; set; }
        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }
        public Users? User { get; set; }
        public string? Type { get; set; } 
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Method { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
        public int? Receipt_No { get; set; }
        public DateTime? Deadline { get; set; }
        public int? Penalty { get; set; }

    }
}
