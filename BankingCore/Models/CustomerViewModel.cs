using System.ComponentModel.DataAnnotations;

namespace BankingCore.Models
{
    public class CustomerViewModel
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(15)]
        public string Phone { get; set; }
    }
}
