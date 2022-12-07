using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("hotelcancun.booking")]
    public class Booking
    {
        public int IdBooking { get; set; }
        public int IdRoom { get; set; }
        public int IdClient { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
    }
}
