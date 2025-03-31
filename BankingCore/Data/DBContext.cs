using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BankingCore.Data;
using BankingCore.Models;

namespace BankingCore.Data
{
    public static class DBContext
    {
        public static CustomerDB customerDBContext { get; set; }        
        static DBContext()
        {
            var configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

            string connectionString = configuration.GetConnectionString("CustomerDBConnection");

            var options = new DbContextOptionsBuilder<CustomerDB>()
           .UseSqlServer(connectionString) // Replace with your connection string
           .Options;
            customerDBContext = new CustomerDB(options);

        }
    }
}
