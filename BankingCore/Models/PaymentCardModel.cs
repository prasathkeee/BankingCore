using System.ComponentModel.DataAnnotations;

namespace BankingCore.Models
{
    public class PaymentCardModel
    {
        public int SelectedOptionId { get; set; } // Stores the selected option
        public List<Option> Options { get; set; } // List of options for the dropdown

        [Required]
        [Display(Name = "Cardholder Name")]
        public string CardholderName { get; set; }

        [Required]
        [CreditCard]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        [Display(Name = "Expiration Month")]
        public int ExpirationMonth { get; set; }

        [Required]
        [Range(2023, 2050, ErrorMessage = "Year must be valid.")]
        [Display(Name = "Expiration Year")]
        public int ExpirationYear { get; set; }

        [Required]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Invalid CVV.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }
    }
    public class Option
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
