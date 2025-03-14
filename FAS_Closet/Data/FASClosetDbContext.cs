// using Microsoft.EntityFrameworkCore;
// using FASCloset.Models;

// namespace FASCloset.Data
// {
//     public class FASClosetDbContext : DbContext
//     {
//         public DbSet<Manufacturer> Manufacturers { get; set; }
//         public DbSet<Category> Categories { get; set; }
//         public DbSet<Product> Products { get; set; }
//         public DbSet<Customer> Customers { get; set; }
//         public DbSet<Order> Orders { get; set; }
//         public DbSet<OrderDetail> OrderDetails { get; set; }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             optionsBuilder.UseSqlite("Data Source=fascloset.db");
//         }
//     }
// }
