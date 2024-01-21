using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
    public class Room
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("room_id")]
        public int? room_id { get; set; }
        public string? type{ get; set; }
        public int? floor { get; set; }
        public string? status { get; set; }
    }
}
