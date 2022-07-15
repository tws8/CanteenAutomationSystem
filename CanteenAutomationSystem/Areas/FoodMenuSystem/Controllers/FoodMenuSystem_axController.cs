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

namespace CanteenAutomationSystem.Areas.FoodMenuSystem.Controllers
{
    public class FoodMenuSystem_axController : BaseController
    {
        [Authorize]
        public ActionResult ax_Menu()
        {
            var gSetting = _gSetting();

            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);
            string status = pRTIN(Request.QueryString["fmstatus"]);
            
            using (var context = new CanteenContext())
            {
                var qFood = from x in context.Foods
                            orderby x.Category
                            select x;

                return View(ax_FoodMenuSystem(qFood.ToList()));
            }
        }

        [Authorize]
        public DataTable ax_FoodMenuSystem(List<Food> lstFood)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("FoodID", typeof(Int32));
            dt.Columns.Add("FoodDesc", typeof(String));
            dt.Columns.Add("FoodPrice", typeof(Decimal));
            dt.Columns.Add("FoodCurOrdQty", typeof(Int32));
            dt.Columns.Add("FoodRem", typeof(String));
            dt.Columns.Add("Category", typeof(String));
            dt.Columns.Add("Url", typeof(String));
            dt.Columns.Add("Status", typeof(String));

            using (var context = new CanteenContext())
            {
                string sUserName = pConvStr(Session["_USERNAME"]);
                if (context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.Status.Equals("A")).Any())
                {
                    TempData["ACTORDER"] = "Y";
                }
                else
                {
                    TempData["ACTORDER"] = "N";
                }

                int iQty;
                var qOrderDet = from y in context.OrderDets
                                where y.OrderID.Equals(context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.Status.Equals("A")).Select(x => x.OrderID).FirstOrDefault())
                                select new { y.FoodID, y.Quantity};

                foreach (var food in lstFood)
                {
                    if (TempData["ACTORDER"].ToString().Equals("Y"))
                    {
                        var qFood = qOrderDet.Where(x => x.FoodID.Equals(food.FoodID));
                        if (qFood.Any())
                        {
                            iQty = qFood.Select(x => x.Quantity).First();
                        }
                        else
                        {
                            iQty = 0;
                        }
                    }
                    else
                    {
                        iQty = 0;
                    }

                    dr = dt.NewRow();
                    dr.SetField<Int32>("FoodID", food.FoodID);
                    dr.SetField<String>("FoodDesc", food.FoodDesc);
                    dr.SetField<Decimal>("FoodPrice", food.FoodPrice);
                    dr.SetField<Int32>("FoodCurOrdQty", iQty);
                    dr.SetField<String>("FoodRem", food.FoodRem);
                    dr.SetField<String>("Category", food.Category);
                    dr.SetField<String>("Url", food.Url);
                    dr.SetField<String>("Status", food.Status);

                    dt.Rows.Add(dr);
                }
                return dt;
            }

        }

        [Authorize]
        public ActionResult ax_FoodDet()
        {
            var gSetting = _gSetting();
            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);
            string status = pRTIN(Request.QueryString["fmstatus"]);
            int id = pConvInt(Request.QueryString["sRefNo"]);

            TempData["STATUS"] = status;
            TempData["vCONTENT"] = "ax_FoodDet";

            using (var context = new CanteenContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("FoodID", typeof(Int32));
                dt.Columns.Add("FoodDetID", typeof(Int32));
                dt.Columns.Add("IngredientID", typeof(Int32));
                dt.Columns.Add("IngredientDesc", typeof(String));
                dt.Columns.Add("IngredientQty", typeof(Int32));
                dt.Columns.Add("Status", typeof(String));

                DataRow dr;

                var qFoodDet = from x in context.FoodDets
                           join y in context.Ingredients on x.IngredientID equals y.IngredientID
                           where x.FoodID.Equals(id)
                           orderby x.FoodDetID
                           select new { x, y.IngredientDesc };

                foreach (var foodDet in qFoodDet)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("FoodID", foodDet.x.FoodID);
                    dr.SetField<Int32>("FoodDetID", foodDet.x.FoodDetID);
                    dr.SetField<Int32>("IngredientID", foodDet.x.IngredientID);
                    dr.SetField<String>("IngredientDesc", foodDet.IngredientDesc);
                    dr.SetField<Int32>("IngredientQty", foodDet.x.IngredientQty);
                    dr.SetField<String>("Status", foodDet.x.Status);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gFmPageSize, page, search, dt));
            }
        }

        public ActionResult ax_Category()
        {
            var gSetting = _gSetting();

            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);

            using (var context = new CanteenContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CategoryID", typeof(String));
                dt.Columns.Add("CategoryDesc", typeof(String));

                DataRow dr;
                IQueryable<Category> qCategory;

                if (!string.IsNullOrEmpty(search))
                {
                    qCategory = context.Categorys.Where(x => x.CategoryID.Contains(search) || x.CategoryDesc.Contains(search)).OrderByDescending(x => x.CategoryID).Select(x => x);

                }
                else
                {
                    qCategory = context.Categorys.OrderByDescending(x => x.CategoryID).Select(x => x);
                }

                foreach (var category in qCategory)
                {
                    dr = dt.NewRow();
                    dr.SetField<String>("CategoryID", category.CategoryID);
                    dr.SetField<String>("CategoryDesc", category.CategoryDesc);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gVwPageSize, page, search, dt));
            }

        }

        public ActionResult ax_Ingredient()
        {
            var gSetting = _gSetting();

            int page = pConvInt(Request.QueryString["page"]);
            string search = pRTIN(Request.QueryString["search"]);

            using (var context = new CanteenContext())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("IngredientID", typeof(Int32));
                dt.Columns.Add("IngredientDesc", typeof(String));

                DataRow dr;
                IQueryable<Ingredient> qIngredient;

                if (!string.IsNullOrEmpty(search))
                {
                    qIngredient = context.Ingredients.Where(x => x.IngredientID.ToString().Contains(search) || x.IngredientDesc.Contains(search)).OrderByDescending(x => x.IngredientID).Select(x => x);

                }
                else
                {
                    qIngredient = context.Ingredients.OrderByDescending(x => x.IngredientID).Select(x => x);
                }

                foreach (var category in qIngredient)
                {
                    dr = dt.NewRow();
                    dr.SetField<Int32>("IngredientID", category.IngredientID);
                    dr.SetField<String>("IngredientDesc", category.IngredientDesc);

                    dt.Rows.Add(dr);
                }

                return View(IniVwListing(gSetting.gVwPageSize, page, search, dt));
            }

        }

    }
}