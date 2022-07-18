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

namespace CanteenAutomationSystem.Areas.RecipeManagementSystem.Controllers
{
    public class RecipeManagementSystem_axController : BaseController
    {
        [Authorize]
        public ActionResult ax_PurchaseOrder()
        {
            var gSetting = _gSetting();

            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);
            
            using (var context = new CanteenContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PurchaseOrderID", typeof(Int32));
                dt.Columns.Add("Vendor", typeof(Int32));
                dt.Columns.Add("TotalPrice", typeof(Decimal));
                dt.Columns.Add("Status", typeof(String));

                DataRow dr;
                IQueryable<PurchaseOrder> qPurchaseOrder;

                if (!string.IsNullOrEmpty(search))
                {
                    qPurchaseOrder = from x in context.PurchaseOrders
                                     where x.PurchaseOrderID.ToString().Contains(search)
                                     orderby x.PurchaseOrderID descending
                                     select x;
                }
                else
                {
                    qPurchaseOrder = from x in context.PurchaseOrders
                                     orderby x.PurchaseOrderID descending
                                     select x;
                }

                foreach (var purchaseOrder in qPurchaseOrder)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("PurchaseOrderID", purchaseOrder.PurchaseOrderID);
                    dr.SetField<Int32>("Vendor", purchaseOrder.Vendor);
                    dr.SetField<Decimal>("TotalPrice", purchaseOrder.TotalPrice);
                    dr.SetField<String>("Status", purchaseOrder.Status);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gVwPageSize, page, search, dt));
            }
        }

        [Authorize]
        public ActionResult ax_PurchaseOrderDet()
        {
            var gSetting = _gSetting();
            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);
            int id = pConvInt(Request.QueryString["sRefNo"]);

            TempData["vCONTENT"] = "ax_PurchaseOrderDet";

            using (var context = new CanteenContext())
            {
                if (context.PurchaseOrders.Where(x => x.PurchaseOrderID.Equals(id)).Select(x => x.Status).Any())
                {
                    TempData["STATUS"] = context.PurchaseOrders.Where(x => x.PurchaseOrderID.Equals(id)).Select(x => x.Status).First();
                }
                else
                {
                    TempData["STATUS"] = "";
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("PurchaseOrderID", typeof(Int32));
                dt.Columns.Add("PurchaseOrderDetID", typeof(Int32));
                dt.Columns.Add("IngredientID", typeof(Int32));
                dt.Columns.Add("IngredientDesc", typeof(String));
                dt.Columns.Add("UnitPrice", typeof(Decimal));
                dt.Columns.Add("Quantity", typeof(Int32));
                dt.Columns.Add("IngredientPrice", typeof(Decimal));
                dt.Columns.Add("Status", typeof(String));

                DataRow dr;

                var qOrderDet = from x in context.PurchaseOrderDets
                           join y in context.Ingredients on x.IngredientID equals y.IngredientID
                           where x.PurchaseOrderID.Equals(id)
                           orderby x.PurchaseOrderDetID
                           select new { x, y.IngredientDesc };

                foreach (var orderDet in qOrderDet)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("PurchaseOrderID", orderDet.x.PurchaseOrderID);
                    dr.SetField<Int32>("PurchaseOrderDetID", orderDet.x.PurchaseOrderDetID);
                    dr.SetField<Int32>("IngredientID", orderDet.x.IngredientID);
                    dr.SetField<String>("IngredientDesc", orderDet.IngredientDesc);
                    dr.SetField<Decimal>("UnitPrice", orderDet.x.UnitPrice);
                    dr.SetField<Int32>("Quantity", orderDet.x.Quantity);
                    dr.SetField<Decimal>("IngredientPrice", orderDet.x.IngredientPrice);
                    dr.SetField<String>("Status", orderDet.x.Status);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gFmPageSize, page, search, dt));
            }
        }

        public ActionResult ax_Vendor()
        {
            var gSetting = _gSetting();

            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);

            using (var context = new CanteenContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("VendorID", typeof(Int32));
                dt.Columns.Add("VendorDesc", typeof(String));
                dt.Columns.Add("VendorContact", typeof(String));
                dt.Columns.Add("VendorEmail", typeof(String));

                DataRow dr;
                IQueryable<Vendor> qVendor;

                if (!string.IsNullOrEmpty(search))
                {
                    qVendor = context.Vendors.Where(x => x.VendorID.ToString().Contains(search) || x.VendorName.Contains(search) || x.VendorContact.Contains(search) || x.VendorEmail.Contains(search)).OrderByDescending(x => x.VendorID).Select(x => x);

                }
                else
                {
                    qVendor = context.Vendors.OrderByDescending(x => x.VendorID).Select(x => x);
                }

                foreach (var vendor in qVendor)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("VendorID", vendor.VendorID);
                    dr.SetField<String>("VendorDesc", vendor.VendorName);
                    dr.SetField<String>("VendorContact", vendor.VendorContact);
                    dr.SetField<String>("VendorEmail", vendor.VendorEmail);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gVwPageSize, page, search, dt));
            }

        }

    }
}