using LiquefiedPetroleumGas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LiquefiedPetroleumGas.Infrastructure
{
    public class LiquefiedPetroleumGasLoginAndRegistrationContext : IdentityDbContext<Users>
    {
        public LiquefiedPetroleumGasLoginAndRegistrationContext(DbContextOptions<LiquefiedPetroleumGasLoginAndRegistrationContext> options) : base(options)
        {

        }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.Entity<UserRole>().ToTable("AspNetUserRoles").HasKey(ur => ur.AgencyId);
            //builder.Entity<UserRole>(b =>
            //{
            //    // Primary key
            //    b.HasKey(u => new { u.AgentId, u.AgencyId });

            //    // Maps to the AspNetUserRoles table
            //    //b.ToTable("AspNetUserRoles");
            //});
        //}

        public DbSet<Page> Pages { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Agencies> Agencies { get; set; }
        public DbSet<Agents> Agents { get; set; }
        //public DbSet<UserRole> userRoles { get; set; }
        //public DbSet<UserLogin> userLogins { get; set; }
        //public DbSet<UserClaim> userClaims { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderStatusCatalog> OrderStatusCatalogs { get; set; }
    }
}
