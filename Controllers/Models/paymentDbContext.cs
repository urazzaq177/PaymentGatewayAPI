using Microsoft.EntityFrameworkCore;

namespace Payment_assignment.DataLayer
{
    public class PaymentDB : DbContext
    {
        public PaymentDB(DbContextOptions<PaymentDB> options) : base(options)
        {  
        }
        public DbSet<payments> payments { get; set; }
        

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyConfiguration(new DeveloperEntityConfiguration());
        //}
    }
}