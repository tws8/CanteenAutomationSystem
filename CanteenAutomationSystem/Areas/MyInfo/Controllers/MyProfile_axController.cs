using CanteenAutomationSystem.Controllers;
using CanteenAutomationSystem.DAL;
using CanteenAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using static CanteenAutomationSystem.Include.Proc;

namespace CanteenAutomationSystem.Areas.MyInfo.Controllers
{
    public class MyProfile_axController : BaseController
    {
        [Authorize]
        public ActionResult ax_Customer()
        {
            var gSetting = _gSetting();

            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);

            using (var context = new CanteenContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CustomerID", typeof(String));
                dt.Columns.Add("CustomerDesc", typeof(String));

                DataRow dr;
                IQueryable<Customer> qCustomer;

                if (!string.IsNullOrEmpty(search))
                {
                    qCustomer = context.Customers.Where(x => x.CustID.Contains(search) || x.CustName.Contains(search)).OrderByDescending(x => x.CustID).Select(x => x);

                }
                else
                {
                    qCustomer = context.Customers.OrderByDescending(x => x.CustID).Select(x => x);
                }

                foreach (var customer in qCustomer)
                {
                    dr = dt.NewRow();
                    dr.SetField<String>("CustomerID", customer.CustID);
                    dr.SetField<String>("CustomerDesc", customer.CustName);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gVwPageSize, page, search, dt));
            }

        }

    }
}