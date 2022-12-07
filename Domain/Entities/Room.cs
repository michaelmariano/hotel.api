using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("hotelcancun.room")]
    public class Room
    {
        public int IdRoom { get; set; }
        public string Number { get; set; }
        public bool IsAvaible { get; set; }
    }
}