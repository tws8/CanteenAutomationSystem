using CanteenAutomationSystem.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem_UnitTest
{
    public class CanteenContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<BizUser> BizUsers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}