using EGreeting.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EGreeting.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Cards> Cards { get; set; }
        public DbSet<CategoriesCard> CategoriesCard { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<OrdersPackage> OrdersPackage { get; set; }
        public DbSet<EmailList> EmailList { get; set; }
        public DbSet<EmailOrders> EmailOrders { get; set; }
        public DbSet<Recharge> Recharge { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmailOrders>()
                .HasMany(e => e.EmailLists)
                .WithOne(el => el.EmailOrder)
                .HasForeignKey(el => el.EmailOrderID)
                .OnDelete(DeleteBehavior.Cascade);

        }


    }
}
