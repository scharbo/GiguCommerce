using Gigu.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigu.Web.DataContext
{
    //public class MyContext : DbContext
    public class GiguContext : IdentityDbContext<Customer, IdentityRole, string>
    {
        public GiguContext(DbContextOptions<GiguContext> options): base(options)
        {

        }

        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderLine> OrderLine { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<IdentityRole> ApplicationRole { get; set; }
    }
}
