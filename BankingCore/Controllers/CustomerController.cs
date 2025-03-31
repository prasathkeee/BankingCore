using BankingCore.Data;
using BankingCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankingCore.Controllers
{
    
    public class CustomerController : Controller
    {
        private CustomerDB _context { get; set; }

        public CustomerController()
        {           
            _context = DBContext.customerDBContext;

        }       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new BillingCustomer
                {
                    TwoLetterIsoCode = "IN",
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    keyInfo = "",
                    APICustomerId = "",
                    CreatedOn = DateTime.Now,
                    PaymentMethodId = 1
                };

                _context.BillingCustomer.Add(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction("Success");
            }

            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
