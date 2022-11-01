using Microsoft.EntityFrameworkCore;
using BankSystemBota.Models;
namespace BankSystemBota.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        public DbSet<DepositInfo> Deposit { get; set; }
        public DbSet<BankSystemBota.Models.CurrencyConverter> CurrencyConverter { get; set; }
    }
}
