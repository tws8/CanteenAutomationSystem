using CanteenAutomationSystem.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem_UnitTest
{
    public class CanteenService
    {
        private CanteenContext _context;

        public CanteenService(CanteenContext context)
        {
            _context = context;
        }

        public List<Customer> GetAllCustomers()
        {
            var query = from x in _context.Customers
                        orderby x.CustName
                        select x;

            return query.ToList();
        }

        public List<BizUser> GetAllBizUsers()
        {
            var query = from x in _context.BizUsers
                        orderby x.BizUserName
                        select x;

            return query.ToList();
        }
    }
}