using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSync.Models
{
    public class Users 
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int UsersId { get; set; }
        [Column("f_name")]
        public required string F_name { get; set; }
        [Column("l_name")]
        public  string? L_name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Preference { get; set; }

        public int? room { get; set; }
        [Column("type")]
        public  string? Type { get; set; }
        [Column("birthdate")]
        public DateTime? BirthDate { get; set; }
        [Column("age")]
        public int? Age { get; set; }


    }
}
