using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
    public class Device
    {
        [Key]
        [Column("device_id")]
        public int DeviceId { get; set; }
        [ForeignKey("room")]
        [Column("room")]
        public int? Room { get; set; }
        [Column("type")]
        public string? Type { get; set; }
        [Column("status")]
        public string? Status { get; set; }
        [Column("battery_status")]
        public int? BatteryStatus { get; set; }
    }
}
