using Domain.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("hotelcancun.client")]
    public class Client
    {
        public int IdClient { get; set; }
        public string FullName { get; set; }
        public string Document { get; set; }
        public string PhoneNumber { get; set; }
        [RegularExpression(RegularExpressions.Email.Pattern, ErrorMessage = RegularExpressions.Email.ErrorMessage)]
        public string Email { get; set; }
        [RegularExpression(RegularExpressions.Password.Pattern, ErrorMessage = RegularExpressions.Password.ErrorMessage)]
        public string Password { get; set; }
    }
}