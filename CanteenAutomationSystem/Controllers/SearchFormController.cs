using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using CanteenAutomationSystem.DAL;
using CanteenAutomationSystem.Models;
using static CanteenAutomationSystem.Include.Proc;

namespace CanteenAutomationSystem.Controllers
{
    [Authorize]
    public class SearchFormController : BaseController
    {
        DataTable dt;
        DataRow dr;

        [Authorize]
        public ActionResult ax_view_Food(string pPage, string pFldName, string pType, string pContent, string pModal, string pSearch, bool pCondition)
        {
            var gSetting = _gSetting();

            using (var context = new CanteenContext())
            {
                dt = new DataTable();
                dt.Columns.Add("FoodID", typeof(Int32));
                dt.Columns.Add("FoodDesc", typeof(String));
                dt.Columns.Add("FoodPrice", typeof(Decimal));
                dt.Columns.Add("FoodRem", typeof(String));
                dt.Columns.Add("Status", typeof(String));

                IQueryable<Food> qFood = context.Foods;
                if (!string.IsNullOrEmpty(pSearch))
                {
                    qFood = qFood.Where(x => x.FoodID.ToString().Contains(pSearch) || 
                                        x.FoodDesc.Contains(pSearch));
                }
                if (pCondition)
                {
                    qFood = qFood.Where(x => x.Status.Equals("A"));
                }
                qFood = qFood.OrderBy(x => x.FoodID).Select(x => x);

                foreach (var food in qFood)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("FoodID", food.FoodID);
                    dr.SetField<String>("FoodDesc", food.FoodDesc);
                    dr.SetField<Decimal>("FoodPrice", food.FoodPrice);
                    dr.SetField<String>("FoodRem", food.FoodRem);
                    dr.SetField<String>("Status", food.Status);

                    dt.Rows.Add(dr);
                }
                return View(IniVwSrcListing(gSetting.gVwPageSize, pConvInt(pPage), pFldName, pType, pContent, pModal, pSearch, dt));
            }
        }

        [Authorize]
        public ActionResult ax_view_Ingredient(string pPage, string pFldName, string pType, string pContent, string pModal, string pSearch, bool pCondition)
        {
            var gSetting = _gSetting();

            using (var context = new CanteenContext())
            {
                dt = new DataTable();
                dt.Columns.Add("IngredientID", typeof(Int32));
                dt.Columns.Add("IngredientDesc", typeof(String));
                dt.Columns.Add("IngredientQty", typeof(Int32));

                IQueryable<Ingredient> qIngredient = context.Ingredients;
                if (!string.IsNullOrEmpty(pSearch))
                {
                    qIngredient = qIngredient.Where(x => x.IngredientID.ToString().Contains(pSearch) ||
                                        x.IngredientDesc.Contains(pSearch));
                }
                qIngredient = qIngredient.OrderBy(x => x.IngredientID).Select(x => x);

                foreach (var food in qIngredient)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("IngredientID", food.IngredientID);
                    dr.SetField<String>("IngredientDesc", food.IngredientDesc);
                    dr.SetField<Int32>("IngredientQty", food.IngredientQty);

                    dt.Rows.Add(dr);
                }
                return View(IniVwSrcListing(gSetting.gVwPageSize, pConvInt(pPage), pFldName, pType, pContent, pModal, pSearch, dt));
            }
        }

        [Authorize]
        public ActionResult ax_view_Vendor(string pPage, string pFldName, string pType, string pContent, string pModal, string pSearch, bool pCondition)
        {
            var gSetting = _gSetting();

            using (var context = new CanteenContext())
            {
                dt = new DataTable();
                dt.Columns.Add("VendorID", typeof(Int32));
                dt.Columns.Add("VendorName", typeof(String));
                dt.Columns.Add("VendorPhone", typeof(String));
                dt.Columns.Add("VendorEmail", typeof(String));

                IQueryable<Vendor> qVendor = context.Vendors;
                if (!string.IsNullOrEmpty(pSearch))
                {
                    qVendor = qVendor.Where(x => x.VendorID.ToString().Contains(pSearch) ||
                                        x.VendorName.Contains(pSearch) ||
                                        x.VendorContact.Contains(pSearch) ||
                                        x.VendorEmail.Contains(pSearch)
                                        );
                }
                qVendor = qVendor.OrderBy(x => x.VendorID).Select(x => x);

                foreach (var vendor in qVendor)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("VendorID", vendor.VendorID);
                    dr.SetField<String>("VendorName", vendor.VendorName);
                    dr.SetField<String>("VendorPhone", vendor.VendorContact);
                    dr.SetField<String>("VendorEmail", vendor.VendorEmail);

                    dt.Rows.Add(dr);
                }
                return View(IniVwSrcListing(gSetting.gVwPageSize, pConvInt(pPage), pFldName, pType, pContent, pModal, pSearch, dt));
            }
        }

    }
}