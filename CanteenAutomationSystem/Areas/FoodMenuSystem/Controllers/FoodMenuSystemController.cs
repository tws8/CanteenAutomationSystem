using CanteenAutomationSystem.Areas.FoodMenuSystem.Models;
using CanteenAutomationSystem.Controllers;
using CanteenAutomationSystem.DAL;
using CanteenAutomationSystem.Models;
using CanteenAutomationSystem.Variables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Linq;
using System.Web.Mvc;
using static CanteenAutomationSystem.Include.Proc;

namespace CanteenAutomationSystem.Areas.FoodMenuSystem.Controllers
{
    [Authorize]
    public class FoodMenuSystemController : BaseController
    {
        gSetting gSetting;

        [Authorize]
        public ActionResult MenuListing()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            try
            {
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
                }
            }
            finally
            {
            }

            return View();
        }

        [Authorize]
        public ActionResult MenuJS()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            FoodMenu iniTBL = new FoodMenu() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);
            TempData["TYPE"] = pRTIN(Request.QueryString["type"]);

            int iID = pConvInt(Request.QueryString["FoodID"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    if (!string.IsNullOrEmpty(TempData["TYPE"].ToString()) && !TempData["TYPE"].ToString().Equals("N"))
                    {
                        var qMenuHeader = context.Foods.Where(x => x.FoodID.Equals(iID));

                        iniTBL.ID = qMenuHeader.Select(x => x.FoodID).First();
                        iniTBL.DESC = qMenuHeader.Select(x => x.FoodDesc).First();
                        iniTBL.PRICE = qMenuHeader.Select(x => x.FoodPrice).First();
                        iniTBL.REM = qMenuHeader.Select(x => x.FoodRem).First();
                        iniTBL.CATEGORY = qMenuHeader.Select(x => x.Category).First();
                        iniTBL.URL = qMenuHeader.Select(x => x.Url).First();
                        iniTBL.STATUS = qMenuHeader.Select(x => x.Status).First();
                    }
                    else
                    {
                        var qMenuHeader = context.Foods.OrderByDescending(x => x.FoodID);

                        iniTBL.ID = qMenuHeader.Select(x => x.FoodID).First() + 1;
                    }

                    ViewData["CATEGORY"] = context.Categorys.ToList();
                }
            }
            finally
            {
            }

            return View(iniTBL);
        }

        [Authorize]
        public ActionResult CategoryListing()
        {
            //------------------------------
            iniTempData();  //Reset TempData
            //------------------------------

            if (pConvInt(Request.QueryString["page"]) == 0)
            {
                TempData["PAGECONT"] = 1;
            }
            else
            {
                TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            }
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            return View();
        }

        [Authorize]
        public ActionResult CategoryData()
        {
            //------------------------------
            iniTempData();  //Reset TempData
            //------------------------------

            MenuCategory iniTBL = new MenuCategory() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);
            TempData["TYPE"] = pRTIN(Request.QueryString["type"]);

            string sID = pRTIN(Request.QueryString["CategoryID"]);

            try
            {
                if (!string.IsNullOrEmpty(TempData["TYPE"].ToString()) && !TempData["TYPE"].ToString().Equals("N"))
                {
                    using (var context = new CanteenContext())
                    {
                        var qCategory = context.Categorys.Where(x => x.CategoryID.Equals(sID)).First();

                        iniTBL.ID = qCategory.CategoryID;
                        iniTBL.DESC = qCategory.CategoryDesc;
                    }
                }
            }
            finally
            {
            }
            return View(iniTBL);
        }

        [Authorize]
        public ActionResult IngredientListing()
        {
            //------------------------------
            iniTempData();  //Reset TempData
            //------------------------------

            if (pConvInt(Request.QueryString["page"]) == 0)
            {
                TempData["PAGECONT"] = 1;
            }
            else
            {
                TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            }
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            return View();
        }

        [Authorize]
        public ActionResult IngredientData()
        {
            //------------------------------
            iniTempData();  //Reset TempData
            //------------------------------

            Models.Ingredient iniTBL = new Models.Ingredient() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);
            TempData["TYPE"] = pRTIN(Request.QueryString["type"]);

            int iID = pConvInt(Request.QueryString["IngredientID"]);

            try
            {
                if (!string.IsNullOrEmpty(TempData["TYPE"].ToString()) && !TempData["TYPE"].ToString().Equals("N"))
                {
                    using (var context = new CanteenContext())
                    {
                        var qIngredient = context.Ingredients.Where(x => x.IngredientID.Equals(iID)).First();

                        iniTBL.ID = qIngredient.IngredientID;
                        iniTBL.DESC = qIngredient.IngredientDesc;
                        iniTBL.QTY = qIngredient.IngredientQty;
                    }
                }
                else
                {
                    using (var context = new CanteenContext())
                    {
                        var qIngredient = context.Ingredients.OrderByDescending(x => x.IngredientID).First();
                        iniTBL.ID = qIngredient.IngredientID + 1;
                    }
                }
            }
            finally
            {
            }
            return View(iniTBL);
        }

        [HttpGet]
        public JsonResult MenuContent()
        {
            int iID = pConvInt(Request.QueryString["iID"]);
            int iNUM = pConvInt(Request.QueryString["iNUM"]);

            FoodIngredient iniTBL = new FoodIngredient() { };

            try
            {
                using (var context = new CanteenContext())
                {
                    var qFoodMenu = context.FoodDets.Where(x => x.FoodID.Equals(iID)).Where(x => x.FoodDetID.Equals(iNUM)).Select(x => x).First();

                    iniTBL.ID = iID;
                    iniTBL.FOODDETID = iNUM;
                    iniTBL.INGREDIENTID = qFoodMenu.IngredientID;
                    iniTBL.INGREDIENTDESC = context.Ingredients.Where(x => x.IngredientID.Equals(qFoodMenu.IngredientID)).First().IngredientDesc;
                    iniTBL.INGREDIENTQTY = qFoodMenu.IngredientQty;

                }
            }
            finally
            {
            }
            return Json(iniTBL, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult _INGREDIENT(string sIngredientID)
        {
            int iIngredientID = pConvInt(sIngredientID);
            TempDTO iniTBL = new TempDTO() { };

            try
            {
                using (var context = new CanteenContext())
                {
                    if (context.Ingredients.Where(x => x.IngredientID.Equals(iIngredientID)).Any())
                    {
                        var qIngredient = context.Ingredients.Where(x => x.IngredientID.Equals(iIngredientID)).Select(x => x).First();

                        iniTBL.str1 = qIngredient.IngredientDesc;
                        iniTBL.int1 = qIngredient.IngredientQty;
                    }
                    else
                    {
                        iniTBL.str1 = "";
                        iniTBL.int1 = 0;
                    }
                }
            }
            finally
            {
            }

            return Json(iniTBL, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddFood_Chk(FoodMenu FoodMenu)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(FoodMenu.ID);
                string sCATEGORY = pRTIN(FoodMenu.CATEGORY);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (context.Foods.Where(x => x.FoodID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID used"));
                        }

                        if (!context.Categorys.Where(x => x.CategoryID.Equals(sCATEGORY)).Any())
                        {
                            return Json(Ermsg("Category not found"));
                        }

                        return Json("OK");
                    }
                }
                catch (Exception ex)
                {
                    return Json(Ermsg(ex.Message));
                }
            }
            else
            {
                allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(allErrors);
            }
        }

        [HttpPost]
        public JsonResult UpdateFood_Chk(FoodMenu FoodMenu)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(FoodMenu.ID);
                string sCATEGORY = pRTIN(FoodMenu.CATEGORY);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.Foods.Where(x => x.FoodID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID not found"));
                        }

                        if (!context.Categorys.Where(x => x.CategoryID.Equals(sCATEGORY)).Any())
                        {
                            return Json(Ermsg("Category not found"));
                        }

                        return Json("OK");
                    }
                }
                catch (Exception ex)
                {
                    return Json(Ermsg(ex.Message));
                }
            }
            else
            {
                allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(allErrors);
            }
        }

        [HttpPost]
        public JsonResult AddCategory_Chk(MenuCategory MenuCategory)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                string sID = pRTIN(MenuCategory.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (context.Categorys.Where(x => x.CategoryID.Equals(sID)).Any())
                        {
                            return Json(Ermsg("ID used"));
                        }

                        return Json("OK");
                    }
                }
                catch (Exception ex)
                {
                    return Json(Ermsg(ex.Message));
                }
            }
            else
            {
                allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(allErrors);
            }
        }

        [HttpPost]
        public JsonResult UpdateCategory_Chk(MenuCategory MenuCategory)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                string sID = pRTIN(MenuCategory.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.Categorys.Where(x => x.CategoryID.Equals(sID)).Any())
                        {
                            return Json(Ermsg("ID not found"));
                        }

                        return Json("OK");
                    }
                }
                catch (Exception ex)
                {
                    return Json(Ermsg(ex.Message));
                }
            }
            else
            {
                allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(allErrors);
            }
        }

        [HttpPost]
        public JsonResult AddIngredientDet_Chk(Models.Ingredient Ingredient)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Ingredient.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (context.Ingredients.Where(x => x.IngredientID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID used"));
                        }

                        return Json("OK");
                    }
                }
                catch (Exception ex)
                {
                    return Json(Ermsg(ex.Message));
                }
            }
            else
            {
                allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(allErrors);
            }
        }

        [HttpPost]
        public JsonResult UpdateIngredientDet_Chk(Models.Ingredient Ingredient)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Ingredient.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.Ingredients.Where(x => x.IngredientID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID not found"));
                        }

                        return Json("OK");
                    }
                }
                catch (Exception ex)
                {
                    return Json(Ermsg(ex.Message));
                }
            }
            else
            {
                allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Json(allErrors);
            }
        }

        [HttpPost]
        public JsonResult AddIngredient_Chk(int iID, int iNum, int iSubID)
        {
            try
            {
                using (var context = new CanteenContext())
                {
                    if (!context.Foods.Where(x => x.FoodID.Equals(iID)).Any())
                    {
                        return Json(Ermsg("ID not found"));
                    }

                    if (context.FoodDets.Where(x => x.FoodID.Equals(iID)).Where(x => x.FoodDetID.Equals(iNum)).Any())
                    {
                        return Json(Ermsg("Number used"));
                    }

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateIngredient_Chk(int iID, int iNum, int iSubID)
        {
            try
            {
                using (var context = new CanteenContext())
                {
                    if (!context.Foods.Where(x => x.FoodID.Equals(iID)).Any())
                    {
                        return Json(Ermsg("ID not found"));
                    }

                    if (!context.FoodDets.Where(x => x.FoodID.Equals(iID)).Where(x => x.FoodDetID.Equals(iNum)).Any())
                    {
                        return Json(Ermsg("Number not found"));
                    }

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [Authorize]
        public ActionResult BuildOrder(FormCollection formCollection)
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    gSetting = new gSetting();
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    int iNewOrder = (context.Orders.Where(x => x.Customer.Equals(sUserName)).Any() ? pConvInt(context.Orders.Where(x => x.Customer.Equals(sUserName)).OrderByDescending(x => x.OrderID).Select(x => x.OrderID).First()) + 1 : 1), iID, iNum = 1, iQty;
                    bool bFirst = true, bDiscount = context.Customers.Where(x => x.CustID.Equals(sUserName)).Select(x => x.CustMemberStatus).First().Equals("Y");
                    decimal dcUnitPrice, dcTotPrice;

                    foreach (string key in formCollection.AllKeys)
                    {
                        iQty = pConvInt(formCollection[key].ToString());

                        if (!iQty.Equals(0))
                        {
                            iID = pConvInt(key.Remove(0, key.IndexOf("_") + 1));
                            dcUnitPrice = context.Foods.Where(x => x.FoodID.Equals(iID)).Select(x => x.FoodPrice).First();
                            dcTotPrice = (pConvDec(iQty) * dcUnitPrice);

                            if (bFirst)
                            {
                                var orders = new Order()
                                {
                                    OrderID = iNewOrder,
                                    Customer = sUserName,
                                    Delivery = "", //To be determined during order confirmation
                                    TotalPrice = 0, //To be calculated after the end of loop
                                    Discount = 0, //To be calculated after the end of loop
                                    ActPrice = 0, //To be calculated after the end of loop
                                    CustRating = null, //To be determined by Customer whether to provide the rating
                                    Status = "A",

                                };
                                context.Orders.Add(orders);
                                context.SaveChanges();

                                bFirst = false;
                            }

                            var orderDets = new OrderDet()
                            {
                                OrderID = iNewOrder,
                                OrderDetID = iNum,
                                FoodID = iID,
                                UnitPrice = dcUnitPrice,
                                Quantity = iQty,
                                TotalPrice = dcTotPrice,
                                Status = "A",

                            };
                            context.OrderDets.Add(orderDets);
                            context.SaveChanges();
                            iNum++;
                        }

                    }

                    Order_Recalculation(iNewOrder);
                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [Authorize]
        public ActionResult UpdateOrder(FormCollection formCollection)
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    int iOrder = context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.Status.Equals("A")).Select(x => x.OrderID).First(),
                        iNum = context.OrderDets.Where(x => x.OrderID.Equals(iOrder)).OrderByDescending(x => x.OrderDetID).Select(x => x.OrderDetID).First() + 1,
                        iID, iQty;
                    decimal dcUnitPrice, dcTotPrice;

                    List<OrderDet> orderDets = context.OrderDets.Where(x => x.OrderID.Equals(iOrder)).Select(x => x).ToList();
                    foreach (string key in formCollection.AllKeys)
                    {
                        iID = pConvInt(key.Remove(0, key.IndexOf("_") + 1));
                        var qFood = orderDets.Where(x => x.FoodID.Equals(iID));
                        dcUnitPrice = context.Foods.Where(x => x.FoodID.Equals(iID)).Select(x => x.FoodPrice).First();
                        iQty = pConvInt(formCollection[key].ToString());
                        dcTotPrice = (pConvDec(iQty) * dcUnitPrice);

                        if (qFood.Any())
                        {
                            qFood.First().UnitPrice = dcUnitPrice;
                            qFood.First().Quantity = iQty;
                            qFood.First().TotalPrice = dcTotPrice;

                            if (!iQty.Equals(0))
                            {
                                qFood.First().Status = "A";
                            }
                            else
                            {
                                qFood.First().Status = "D";
                            }
                            context.SaveChanges();
                        }
                        else
                        {
                            if (!iQty.Equals(0))
                            {
                                var orderNewDets = new OrderDet()
                                {
                                    OrderID = iOrder,
                                    OrderDetID = iNum,
                                    FoodID = iID,
                                    UnitPrice = dcUnitPrice,
                                    Quantity = iQty,
                                    TotalPrice = dcTotPrice,
                                    Status = "A",


                                };
                                context.OrderDets.Add(orderNewDets);
                                context.SaveChanges();
                                iNum++;
                            }
                        }
                    }

                    Order_Recalculation(iOrder);
                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult AddFood(FoodMenu FoodMenu)
        {
            int iID = pConvInt(FoodMenu.ID);
            string sDESC = pRTIN(FoodMenu.DESC);
            decimal dcPRICE = pConvDec(FoodMenu.PRICE);
            string sREM = pRTIN(FoodMenu.REM);
            string sCATEGORY = pRTIN(FoodMenu.CATEGORY);
            string sURL = pRTIN(FoodMenu.URL);
            string sSTATUS = pRTIN(FoodMenu.STATUS);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = new Food()
                    {
                        FoodID = iID,
                        FoodDesc = sDESC,
                        FoodPrice = dcPRICE,
                        FoodRem = sREM,
                        Category = sCATEGORY,
                        Url = sURL,
                        Status = sSTATUS,

                    };
                    context.Foods.Add(foods);
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateFood(FoodMenu FoodMenu)
        {
            int iID = pConvInt(FoodMenu.ID);
            string sDESC = pRTIN(FoodMenu.DESC);
            decimal dcPRICE = pConvDec(FoodMenu.PRICE);
            string sREM = pRTIN(FoodMenu.REM);
            string sCATEGORY = pRTIN(FoodMenu.CATEGORY);
            string sURL = pRTIN(FoodMenu.URL);
            string sSTATUS = pRTIN(FoodMenu.STATUS);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.Foods.First(x => x.FoodID.Equals(iID));
                    foods.FoodDesc = sDESC;
                    foods.FoodPrice = dcPRICE;
                    foods.FoodRem = sREM;
                    foods.Category = sCATEGORY;
                    foods.Url = sURL;
                    foods.Status = sSTATUS;
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult DeleteFood(FoodMenu FoodMenu)
        {
            int iID = pConvInt(FoodMenu.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foodDets = context.FoodDets.Where(x => x.FoodID.Equals(iID));
                    foreach (var foodDet in foodDets)
                    {
                        foodDet.Status = "D";
                    }
                    context.SaveChanges();

                    var foods = context.Foods.First(x => x.FoodID.Equals(iID));
                    foods.Status = "D";
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult OnFood(FoodMenu FoodMenu)
        {
            int iID = pConvInt(FoodMenu.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.Foods.First(x => x.FoodID.Equals(iID));
                    foods.Status = "A";
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult OffFood(FoodMenu FoodMenu)
        {
            int iID = pConvInt(FoodMenu.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.Foods.First(x => x.FoodID.Equals(iID));
                    foods.Status = "I";
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult AddCategory(MenuCategory MenuCategory)
        {
            string sID = pRTIN(MenuCategory.ID);
            string sDESC = pRTIN(MenuCategory.DESC);

            try
            {
                using (var context = new CanteenContext())
                {
                    var categorys = new Category()
                    {
                        CategoryID = sID,
                        CategoryDesc = sDESC,

                    };
                    context.Categorys.Add(categorys);
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateCategory(MenuCategory MenuCategory)
        {
            string sID = pRTIN(MenuCategory.ID);
            string sDESC = pRTIN(MenuCategory.DESC);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.Categorys.First(x => x.CategoryID.Equals(sID));
                    foods.CategoryDesc = sDESC;
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult AddIngredient(FoodIngredient FoodIngredient)
        {
            int iID = pConvInt(Request.QueryString["iID"].ToString());
            int iNum = pConvInt(FoodIngredient.FOODDETID);
            int iSubID = pConvInt(FoodIngredient.INGREDIENTID);
            int iQty = pConvInt(FoodIngredient.INGREDIENTQTY);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foodDets = new FoodDet
                    {
                        FoodID = iID,
                        FoodDetID = iNum,
                        IngredientID = iSubID,
                        IngredientQty = iQty,
                        Status = "A",
                    };
                    context.FoodDets.Add(foodDets);
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateIngredient(FoodIngredient FoodIngredient)
        {
            int iID = pConvInt(FoodIngredient.ID);
            int iNum = pConvInt(FoodIngredient.FOODDETID);
            int iSubID = pConvInt(FoodIngredient.INGREDIENTID);
            int iQty = pConvInt(FoodIngredient.INGREDIENTQTY);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.FoodDets.Where(x => x.FoodID.Equals(iID)).Where(x => x.FoodDetID.Equals(iNum)).First();
                    foods.IngredientID = iSubID;
                    foods.IngredientQty = iQty;
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult AddIngredientDet(Models.Ingredient Ingredient)
        {
            int iID = pConvInt(Ingredient.ID);
            string sDesc = pRTIN(Ingredient.DESC);

            try
            {
                using (var context = new CanteenContext())
                {
                    var ingredients = new CanteenAutomationSystem.Models.Ingredient
                    {
                        IngredientID = iID,
                        IngredientDesc = sDesc,
                        IngredientQty = 0,
                    };
                    context.Ingredients.Add(ingredients);
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateIngredientDet(Models.Ingredient Ingredient)
        {
            int iID = pConvInt(Ingredient.ID);
            string sDesc = pRTIN(Ingredient.DESC);

            try
            {
                using (var context = new CanteenContext())
                {
                    var ingredient = context.Ingredients.Where(x => x.IngredientID.Equals(iID)).First();
                    ingredient.IngredientDesc = sDesc;
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult DeleteIngredient(int iID, int iNum)
        {
            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.FoodDets.Where(x => x.FoodID.Equals(iID)).Where(x => x.FoodDetID.Equals(iNum)).First();
                    foods.Status = "D";

                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult NewRecFoodMenu()
        {
            int iID = pConvInt(Request.QueryString["oID"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    int foodDets;
                    if (context.FoodDets.Where(x => x.FoodID.Equals(iID)).Any())
                    {
                        foodDets = context.FoodDets.Where(x => x.FoodID.Equals(iID)).OrderByDescending(x => x.FoodDetID).Select(x => x.FoodDetID).First() + 1;
                    }
                    else
                    {
                        foodDets = 1;
                    }

                    return Json(foodDets);
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult NewRecIngredient()
        {
            int iID = pConvInt(Request.QueryString["oID"]);
            string stype = pRTIN(Request.QueryString["type"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    int ingredients;
                    if(context.Ingredients.Any())
                    {
                        ingredients = context.Ingredients.OrderByDescending(x => x.IngredientID).Select(x => x.IngredientID).First() + 1;
                    }
                    else
                    {
                        ingredients = 1;
                    }


                    return Json(ingredients);
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }
    }
}