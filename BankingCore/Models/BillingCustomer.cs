using System.ComponentModel.DataAnnotations;

namespace BankingCore.Models
{
    public class BillingCustomer
    {
        public int Id { get; set; }
        public string TwoLetterIsoCode { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(15)]
        public string PhoneNumber { get; set; }

        public string keyInfo { get; set; }
        public string APICustomerId { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required, StringLength(5)]
        public int PaymentMethodId { get; set; }
    }
}
