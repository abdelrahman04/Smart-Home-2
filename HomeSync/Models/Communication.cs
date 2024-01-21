using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
    public class Communication
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("message_id")]
        public int MessageId { get; set; }
        [ForeignKey("Receiver")]
        [Column("receiver_id")]
        public int ReceiverId { get; set; }
        public  Users Receiver { get; set; }
        [ForeignKey("Sender")]
        [Column("sender_id")]
        public int SenderId { get; set; }
        public Users Sender { get; set; }
        public string? Content { get; set; }
        public string? Title { get; set; }
        public DateTime? Time_Sent { get; set; }
        public DateTime? Time_Received { get; set; }
        public DateTime? Time_Read { get; set; }


    }
}
