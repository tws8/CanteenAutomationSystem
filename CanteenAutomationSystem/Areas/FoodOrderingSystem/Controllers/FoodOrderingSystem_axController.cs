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

namespace CanteenAutomationSystem.Areas.FoodOrderingSystem.Controllers
{
    public class FoodOrderingSystem_axController : BaseController
    {
        [Authorize]
        public ActionResult ax_FoodOrder()
        {
            var gSetting = _gSetting();

            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);
            
            using (var context = new CanteenContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("OrderID", typeof(Int32));
                dt.Columns.Add("TotalPrice", typeof(Decimal));
                dt.Columns.Add("Discount", typeof(Decimal));
                dt.Columns.Add("ActPrice", typeof(Decimal));
                dt.Columns.Add("Status", typeof(String));

                DataRow dr;
                IQueryable<Order> qOrder;

                if (Session["_USERTYPE"].ToString().Equals("C"))
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    if (!string.IsNullOrEmpty(search))
                    {
                        qOrder = context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => !x.Status.Equals("A")).Where(x => x.OrderID.ToString().Contains(search)).OrderByDescending(x => x.OrderID).Select(x => x);

                    }
                    else
                    {
                        qOrder = context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => !x.Status.Equals("A")).OrderByDescending(x => x.OrderID).Select(x => x);
                    }
                }
                else
                {
                    qOrder = context.Orders.OrderByDescending(x => x.OrderID).Select(x => x);
                }

                foreach (var order in qOrder)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("OrderID", order.OrderID);
                    dr.SetField<Decimal>("TotalPrice", order.TotalPrice);
                    dr.SetField<Decimal>("Discount", order.Discount);
                    dr.SetField<Decimal>("ActPrice", order.ActPrice);
                    dr.SetField<String>("Status", order.Status);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gVwPageSize, page, search, dt));
            }
        }

        [Authorize]
        public ActionResult ax_OrderDet()
        {
            var gSetting = _gSetting();
            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);
            int id = pConvInt(Request.QueryString["sRefNo"]);

            TempData["vCONTENT"] = "ax_OrderDet";

            using (var context = new CanteenContext())
            {
                if (context.Orders.Where(x => x.OrderID.Equals(id)).Select(x => x.Status).Any())
                {
                    TempData["STATUS"] = context.Orders.Where(x => x.OrderID.Equals(id)).Select(x => x.Status).First();
                }
                else
                {
                    TempData["STATUS"] = "";
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("OrderID", typeof(Int32));
                dt.Columns.Add("OrderDetID", typeof(Int32));
                dt.Columns.Add("FoodID", typeof(Int32));
                dt.Columns.Add("FoodDesc", typeof(String));
                dt.Columns.Add("UnitPrice", typeof(Decimal));
                dt.Columns.Add("Quantity", typeof(Int32));
                dt.Columns.Add("TotalPrice", typeof(Decimal));
                dt.Columns.Add("Status", typeof(String));

                DataRow dr;

                var qOrderDet = from x in context.OrderDets
                           join y in context.Foods on x.FoodID equals y.FoodID
                           where x.OrderID.Equals(id)
                           orderby x.OrderDetID
                           select new { x, y.FoodDesc };

                foreach (var orderDet in qOrderDet)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("OrderID", orderDet.x.OrderID);
                    dr.SetField<Int32>("OrderDetID", orderDet.x.OrderDetID);
                    dr.SetField<Int32>("FoodID", orderDet.x.FoodID);
                    dr.SetField<String>("FoodDesc", orderDet.FoodDesc);
                    dr.SetField<Decimal>("UnitPrice", orderDet.x.UnitPrice);
                    dr.SetField<Int32>("Quantity", orderDet.x.Quantity);
                    dr.SetField<Decimal>("TotalPrice", orderDet.x.TotalPrice);
                    dr.SetField<String>("Status", orderDet.x.Status);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gFmPageSize, page, search, dt));
            }
        }

    }
}