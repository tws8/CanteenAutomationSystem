using CanteenAutomationSystem.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.DAL
{
    public class CanteenContext : DbContext
    {
        public CanteenContext() : base("CanteenContext")
        {
        }

        public DbSet<BizDept> BizDepts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BizUser> BizUsers { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodDet> FoodDets { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDet> OrderDets { get; set; }
        public DbSet<Pay> Pays { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDet> PurchaseOrderDets { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}