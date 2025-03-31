namespace BankingCore.Models
{
    public class CardTransactionModel
    {
        public int CustomerId { get; set; }
        public int TransactionId { get; set; }
        public int PaymentMethodId { get; set; }
        public string TransactionType { get; set; }
        public string OrderId { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionReferenceID { get; set; }
        public string TransactionDate { get; set; }
        public string CurrencyCode { get; set; }
        public string CreditCardOwnerName { get; set; }
        public string CreditCardExpireMonth { get; set; }
        public string CreditCardCvv2 { get; set; }

        public string Description { get; set; }
        public string Amount { get; set; }
        public string CreditCardNumber { get; set; }
        public string IsTransactionSuccess { get; set; }
        public string RedirectUrl { get; set; }
        public string TransactionMessage { get; set; }
        public string CreditCardExpireYear { get; set; }

    }
}
