using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("reservations.user")]
    public class User
    {
        public int IdUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}