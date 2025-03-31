using BankingCore.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingCore.Data
{
    public class CustomerDB : DbContext
    {
        public CustomerDB(DbContextOptions<CustomerDB> options) : base(options)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            // Retrieve the connection string
            string connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public DbSet<BillingCustomer> BillingCustomer { get; set; }
        public DbSet<CardTransactionModel> CardTransaction { get; set; }
    }
}
