using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using BankingCore.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BankingCore.Data;

namespace BankingCore.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly string openPayApiUrl = "https://api.openpay.mx/v1/mek7jnqmi3pie04hnwp8/100";
        private readonly string privateKey = "sk_fe645cfeb3dd436bb4c4458030be9106";

        private CustomerDB _context { get; set; }
        public IActionResult Transaction()
        {
            var options = new List<Option>
            {
                new Option { Id = 1, Name = "Electricity Bill" },            
            };

            var model = new PaymentCardModel
            {
                Options = options,
                SelectedOptionId = 1 
            };
            _context = DBContext.customerDBContext;
            return View(model);
        }

       
        [HttpPost]
        public async Task<IActionResult> Transaction(PaymentCardModel model)
        {
            try
            {

                var cardToken = await TokenizeCard(model.CardNumber, model.ExpirationMonth.ToString() + "/" + model.ExpirationYear.ToString(), model.CVV);

                var chargeData = await CreateChargeWith3DSecure(cardToken, model.CardholderName);

                if (chargeData.Needs3DSecure)
                {
                    return Redirect(chargeData.RedirectUrl);
                }
                UpdateTrsactionTable(model);
                return View("Success", chargeData);
            }
            catch (Exception ex)
            {
                return View("Error", "Error in Processing the trasaction - " + ex.Message);
            }
        }
        private void UpdateTrsactionTable(PaymentCardModel model)
        {
            var customer = new CardTransactionModel
            {
                CustomerId = 1,
                TransactionId =1,
                PaymentMethodId =1,
                TransactionType = "",
                OrderId = "",
                TransactionStatus = "",
                TransactionReferenceID = "",
                TransactionDate = DateTime.Now.ToString(),
                CurrencyCode = "",
                CreditCardOwnerName = model.CardholderName,
                CreditCardExpireMonth = model.ExpirationMonth.ToString(),
                CreditCardCvv2 = model.CVV,
                Description = "",
                Amount = "100",
                CreditCardNumber = model.CardNumber,
                IsTransactionSuccess = "",
                RedirectUrl = "",
                TransactionMessage = "Completed",
                CreditCardExpireYear = ""
            };

            _context.CardTransaction.Add(customer);
            _context.SaveChangesAsync();
        }
        private void CreateAPI()
        {
            var publicIp = "192.168.1.6";
            OpenpayAPI openpayAPI = new OpenpayAPI(privateKey, "mek7jnqmi3pie04hnwp8", publicIp, false);

            Customer customer = new Customer();
            customer.Name = "Net Client";
            customer.LastName = "C#";
            customer.Email = "net@c.com";
            customer.Address = new Address();
            customer.Address.Line1 = "line 1";
            customer.Address.PostalCode = "12355";
            customer.Address.City = "Queretaro";
            customer.Address.CountryCode = "MX";
            customer.Address.State = "Queretaro";

            Customer customerCreated = openpayAPI.CustomerService.Create(customer);

            string customer_id = customerCreated.Id;
            Card card = new Card();
            card.CardNumber = "4111111111111111";
            card.BankCode = "002";
            card.HolderName = "Payout User";

            PayoutRequest request = new PayoutRequest();
            request.Method = "card";
            request.Card = card;
            request.Amount = 500.00m;
            request.Description = "Payout test";

            Payout payout = openpayAPI.PayoutService.Create(customer_id, request);
        }
        
        private async Task<string> TokenizeCard(string cardNumber, string expirationDate, string cvv)
        {
            var tokenRequest = new
            {
                card_number = cardNumber,
                expiration_date = expirationDate,
                cvv2 = cvv
            };

            var jsonRequest = JsonConvert.SerializeObject(tokenRequest);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {privateKey}");

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.openpay.mx/v1/mek7jnqmi3pie04hnwp8/cards", content); // Replace with your merchant ID

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseData);
                    return tokenResponse.id;
                }
                else
                {
                    throw new Exception("Failed to tokenize the card.");
                }
            }
        }

        private async Task<dynamic> CreateChargeWith3DSecure(string cardToken, string cardHolderName)
        {
            var chargeRequest = new
            {
                method = "card",
                source_id = cardToken,
                amount = 100.00, 
                description = "Test Transaction",
                device_session_id = Guid.NewGuid().ToString(), 
                redirect_url = "http://www.yoursite.com/transaction/callback",
               
                customer = new
                {
                    name = cardHolderName,
                    email = "customer@example.com" 
                }
            };

            var jsonRequest = JsonConvert.SerializeObject(chargeRequest);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {privateKey}");

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(openPayApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var chargeResponse = JsonConvert.DeserializeObject<dynamic>(responseData);

                    if (chargeResponse.error != null && chargeResponse.error.code == "3D_SECURE_REQUIRED")
                    {
                        return new
                        {
                            Needs3DSecure = true,
                            RedirectUrl = chargeResponse.redirect_url
                        };
                    }

                    return new
                    {
                        Needs3DSecure = false,
                        ChargeData = chargeResponse
                    };
                }
                else
                {
                    throw new Exception("Failed to create charge.");
                }
            }
        }
        [HttpGet]
        public IActionResult Callback(string status)
        {
            if (status == "success")
            {
                return View("Success");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
