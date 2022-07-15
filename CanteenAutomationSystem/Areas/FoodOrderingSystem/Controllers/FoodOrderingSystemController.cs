using CanteenAutomationSystem.Areas.FoodOrderingSystem.Models;
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

namespace CanteenAutomationSystem.Areas.FoodOrderingSystem.Controllers
{
    [Authorize]
    public class FoodOrderingSystemController : BaseController
    {
        [Authorize]
        public ActionResult OrderListing()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            int iID = pConvInt(Request.QueryString["OrderID"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    if (context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iID)).Where(x => x.Status.Equals("A")).Any())
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
        public ActionResult OrderJS()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            Models.Order iniTBL = new Models.Order() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);
            TempData["TYPE"] = pRTIN(Request.QueryString["type"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    if (Request.QueryString["OrderID"] != null)
                    {
                        int iID = pConvInt(Request.QueryString["OrderID"]);
                        if (context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iID)).Any())
                        {
                            Order_Recalculation(iID);

                            var qOrderHeader = context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iID));

                            iniTBL.ID = qOrderHeader.Select(x => x.OrderID).First();
                            iniTBL.DELIVERY = qOrderHeader.Select(x => x.Delivery).First();
                            iniTBL.TOTPRICE = qOrderHeader.Select(x => x.TotalPrice).First();
                            iniTBL.DISCOUNT = qOrderHeader.Select(x => x.Discount).First();
                            iniTBL.ACTPRICE = qOrderHeader.Select(x => x.ActPrice).First();
                            iniTBL.DTORDDATETIME = (DateTime)qOrderHeader.Select(x => x.OrderedDateTime).First();
                            iniTBL.DTESTDATETIME = (DateTime)qOrderHeader.Select(x => x.EstPreparedDateTime).First();
                            iniTBL.DTACTDATETIME = (DateTime)qOrderHeader.Select(x => x.ActPreparedDateTime).First();
                            iniTBL.DTDELDATETIME = (DateTime)qOrderHeader.Select(x => x.ActDeliveryDateTime).First();
                            iniTBL.STATUS = qOrderHeader.Select(x => x.Status).First();
                        }
                        else
                        {
                            var qOrderHeader = context.Orders.Where(x => x.Customer.Equals(sUserName)).OrderByDescending(x => x.OrderID);

                            iniTBL.ID = qOrderHeader.Select(x => x.OrderID).First() + 1;
                        }
                    }
                    else
                    {
                        var qOrderHeader = context.Orders.Where(x => x.Customer.Equals(sUserName)).OrderByDescending(x => x.OrderID);

                        if (qOrderHeader.Any())
                        {
                            iniTBL.ID = qOrderHeader.Select(x => x.OrderID).First() + 1;
                        }
                        else
                        {
                            iniTBL.ID = 1;
                        }
                    }
                }
            }
            finally
            {
            }

            return View(iniTBL);
        }

        [HttpGet]
        public JsonResult OrderContent()
        {
            int iID = pConvInt(Request.QueryString["iID"]);
            int iNUM = pConvInt(Request.QueryString["iNUM"]);

            Models.OrderDet iniTBL = new Models.OrderDet() { };

            try
            {
                using(var context = new CanteenContext())
                {
                    var qFoodOrder = context.OrderDets.Where(x => x.OrderID.Equals(iID)).Where(x => x.OrderDetID.Equals(iNUM)).Select(x => x).First();

                    iniTBL.ID = iID;
                    iniTBL.ORDERDETID = iNUM;
                    iniTBL.FOODID = qFoodOrder.FoodID;
                    iniTBL.FOODDESC = context.Foods.Where(x => x.FoodID.Equals(qFoodOrder.FoodID)).First().FoodDesc;
                    iniTBL.QUANTITY = qFoodOrder.Quantity;
                    iniTBL.TOTPRICE = qFoodOrder.TotalPrice;
                    iniTBL.STATUS = qFoodOrder.Status;

                }
            }
            finally
            {
            }
            return Json(iniTBL, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult _FOOD(string sFoodID)
        {
            int iFoodID = pConvInt(sFoodID);
            TempDTO iniTBL = new TempDTO() { };

            try
            {
                using(var context = new CanteenContext())
                {
                    if (context.Foods.Where(x => x.FoodID.Equals(iFoodID)).Any())
                    {
                        var qFood = context.Foods.Where(x => x.FoodID.Equals(iFoodID)).Select(x => x).First();

                        iniTBL.str1 = qFood.FoodDesc;
                    }
                    else
                    {
                        iniTBL.str1 = "";
                    }
                }
            }
            finally
            {
            }

            return Json(iniTBL, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult _TOTPRICE()
        {
            int iFoodID = pConvInt(Request.QueryString["iID"]);
            int iQty = pConvInt(Request.QueryString["iQty"]);

            TempDTO iniTBL = new TempDTO() { };

            try
            {
                using (var context = new CanteenContext())
                {
                    if (context.Foods.Where(x => x.FoodID.Equals(iFoodID)).Any())
                    {
                        var qFood = context.Foods.Where(x => x.FoodID.Equals(iFoodID)).Select(x => x).First();

                        iniTBL.dec1 = (qFood.FoodPrice * pConvDec(iQty));
                    }
                    else
                    {
                        iniTBL.dec1 = 0;
                    }
                }
            }
            finally
            {
            }

            return Json(iniTBL, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrder_Chk(Models.Order Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        string sUserName = pConvStr(Session["_USERNAME"]);
                        if (context.Orders.Where(x => x.OrderID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID used"));
                        }

                        if (!context.Customers.Where(x => x.CustID.Equals(sUserName)).Any())
                        {
                            return Json(Ermsg("Customer not found"));
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
        public JsonResult UpdateOrder_Chk(Models.Order Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        string sUserName = pConvStr(Session["_USERNAME"]);
                        if (!context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID not found or ID belongs to other Customer"));
                        }

                        if (!context.Customers.Where(x => x.CustID.Equals(sUserName)).Any())
                        {
                            return Json(Ermsg("Customer not found"));
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
        public JsonResult ConfirmOrder_Chk(Models.Order Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);
                decimal dcACTPRICE = pConvDec(Order.ACTPRICE);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        string sUserName = pConvStr(Session["_USERNAME"]);
                        if (!context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID not found or ID belongs to other Customer"));
                        }

                        if (!context.Customers.Where(x => x.CustID.Equals(sUserName)).Any())
                        {
                            return Json(Ermsg("Customer not found"));
                        }

                        if (context.Customers.Where(x => x.CustID.Equals(sUserName)).Select(x => x.BalCredit).First() < dcACTPRICE)
                        {
                            return Json(Ermsg("Balance Credit not enough"));
                        }

                        Dictionary<int, int> dictIngredient = new Dictionary<int, int>();
                        List<Int32> lstOrderDet = context.OrderDets.Where(x => x.OrderID.Equals(iID)).Where(x => x.Status.Equals("A")).Select(x => x.FoodID).ToList();
                        foreach(int orderDet in lstOrderDet)
                        {
                            var qFoodDets = context.FoodDets.Where(x => x.FoodID.Equals(orderDet)).ToList();
                            foreach(var foodDet in qFoodDets)
                            {
                                if (dictIngredient.ContainsKey(foodDet.IngredientID))
                                {
                                    dictIngredient[dictIngredient.First(x => x.Key.Equals(foodDet.IngredientID)).Key] = pConvInt(dictIngredient.First(x => x.Key.Equals(foodDet.IngredientID)).Value) + foodDet.IngredientQty;
                                }
                                else
                                {
                                    dictIngredient.Add(foodDet.IngredientID, foodDet.IngredientQty);
                                }
                                /*
                                if (context.Ingredients.Where(x => x.IngredientID.Equals(foodDet.IngredientID)).Select(x => x.IngredientQty).First() <= foodDet.IngredientQty)
                                {
                                    return Json(Ermsg("Unfortunately " + context.Foods.Where(x => x.FoodID.Equals(foodDet.FoodID)).Select(x => x.FoodDesc).First() +" is out of stock"));
                                }
                                */
                            }
                        }
                        foreach(var ingredient in dictIngredient)
                        {
                            if (context.Ingredients.Where(x => x.IngredientID.Equals(ingredient.Key)).Select(x => x.IngredientQty).First() <= ingredient.Value)
                            {
                                return Json(Ermsg("Unfortunately food is out of stock"));
                            }
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
        public JsonResult PrepareOrder_Chk(Models.Order Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.Orders.Where(x => x.OrderID.Equals(iID)).Where(x => x.Status.Equals("P")).Any())
                        {
                            return Json(Ermsg("ID not found or Order not paid"));
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
        public JsonResult ServeOrder_Chk(Models.Order Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.Orders.Where(x => x.OrderID.Equals(iID)).Where(x => x.Status.Equals("K")).Any())
                        {
                            return Json(Ermsg("ID not found or Order not preparing"));
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
        public JsonResult DeliverOrder_Chk(Models.Order Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.Orders.Where(x => x.OrderID.Equals(iID)).Where(x => x.Status.Equals("R")).Any())
                        {
                            return Json(Ermsg("ID not found or Order not ready"));
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
        public JsonResult AddFood_Chk(int iID, int iNum, int iSubID)
        {
            try
            {
                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    if (!context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iID)).Any())
                    {
                        return Json(Ermsg("ID not found or ID belongs to other Customer"));
                    }

                    if (context.OrderDets.Where(x => x.OrderID.Equals(iID)).Where(x => x.OrderDetID.Equals(iNum)).Any())
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
        public JsonResult UpdateFood_Chk(int iID, int iNum, int iSubID)
        {
            try
            {
                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    if (!context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iID)).Any())
                    {
                        return Json(Ermsg("ID not found or ID belongs to other Customer"));
                    }

                    if (!context.OrderDets.Where(x => x.OrderID.Equals(iID)).Where(x => x.OrderDetID.Equals(iNum)).Any())
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

        [HttpPost]
        public JsonResult UpdateOrder(Models.Order Order)
        {
            int iID = pConvInt(Order.ID);
            string sPAY = pRTIN(Order.PAYMENT);
            string sDEL = pRTIN(Order.DELIVERY);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.Orders.First(x => x.OrderID.Equals(iID));
                    foods.Delivery = sDEL;
                    context.SaveChanges();

                    Order_Recalculation(iID);

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult ConfirmOrder(Models.Order Order)
        {
            int iID = pConvInt(Order.ID);
            decimal dcACTPRICE = pConvDec(Order.ACTPRICE);

            try
            {
                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    DateTime dtNow = DateTime.Now;
                    var foods = context.Orders.First(x => x.OrderID.Equals(iID));
                    foods.OrderedDateTime = dtNow;
                    foods.Status = "P";
                    context.SaveChanges();

                    decimal dcCreditBal = context.Customers.Where(x => x.CustID.Equals(sUserName)).Select(x => x.BalCredit).First();
                    dcCreditBal -= dcACTPRICE;
                    context.SaveChanges();

                    int iPayID = context.Pays.OrderByDescending(x => x.PayID).Select(x => x.PayID).First() + 1;
                    var pays = new Pay()
                    {
                        PayID = iPayID,
                        Customer = sUserName,
                        OrderPrice = iID,

                    };
                    context.Pays.Add(pays);
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
        public JsonResult PrepareOrder(Models.Order Order)
        {
            int iID = pConvInt(Order.ID);
            DateTime dtEst = pConvDate(Order.DTESTDATETIME);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.Orders.First(x => x.OrderID.Equals(iID));
                    foods.EstPreparedDateTime = dtEst;
                    foods.Status = "K";
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
        public JsonResult ServeOrder(Models.Order Order)
        {
            int iID = pConvInt(Order.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    DateTime dtNow = DateTime.Now;
                    var foods = context.Orders.First(x => x.OrderID.Equals(iID));
                    foods.ActPreparedDateTime = dtNow;
                    foods.Status = "R";
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
        public JsonResult DeliverOrder(Models.Order Order)
        {
            int iID = pConvInt(Order.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var foods = context.Orders.First(x => x.OrderID.Equals(iID));
                    foods.Status = "C";
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
        public JsonResult DeleteOrder(Models.Order Order)
        {
            int iID = pConvInt(Order.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var orderDets = context.OrderDets.Where(x => x.OrderID.Equals(iID));
                    foreach(var orderDet in orderDets)
                    {
                        orderDet.Status = "D";
                    }
                    context.SaveChanges();

                    var orders = context.Orders.First(x => x.OrderID.Equals(iID));
                    orders.TotalPrice = 0;
                    orders.Discount = 0;
                    orders.ActPrice = 0;
                    orders.Status = "D";
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
        public JsonResult AddFood(Models.OrderDet OrderDet)
        {
            int iID = pConvInt(Request.QueryString["iID"].ToString());
            int iNum = pConvInt(OrderDet.ORDERDETID);
            int iSubID = pConvInt(OrderDet.FOODID);
            int iQty = pConvInt(OrderDet.QUANTITY);
            decimal dcUnitPrice, dcTotPrice;

            try
            {
                using (var context = new CanteenContext())
                {
                    dcUnitPrice = context.Foods.Where(x => x.FoodID.Equals(iSubID)).Select(x => x.FoodPrice).First();
                    dcTotPrice = pConvDec(iQty) * dcUnitPrice;
                    var orderDets = new CanteenAutomationSystem.Models.OrderDet
                    {
                        OrderID = iID,
                        OrderDetID = iNum,
                        FoodID = iSubID,
                        UnitPrice = dcUnitPrice,
                        Quantity = iQty,
                        TotalPrice = dcTotPrice,
                        Status = "A",
                    };
                    context.OrderDets.Add(orderDets);
                    context.SaveChanges();

                    Order_Recalculation(iID);

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateFood(Models.OrderDet OrderDet)
        {
            int iID = pConvInt(OrderDet.ID);
            int iNum = pConvInt(OrderDet.ORDERDETID);
            int iSubID = pConvInt(OrderDet.FOODID);
            int iQty = pConvInt(OrderDet.QUANTITY);
            decimal dcUnitPrice, dcTotPrice;

            try
            {
                using (var context = new CanteenContext())
                {
                    dcUnitPrice = context.Foods.Where(x => x.FoodID.Equals(iSubID)).Select(x => x.FoodPrice).First();
                    dcTotPrice = pConvDec(iQty) * dcUnitPrice;

                    var orders = context.OrderDets.Where(x => x.OrderID.Equals(iID)).Where(x => x.OrderDetID.Equals(iNum)).First();
                    orders.FoodID = iSubID;
                    orders.UnitPrice = dcUnitPrice;
                    orders.Quantity = iQty;
                    orders.TotalPrice = dcTotPrice;
                    context.SaveChanges();

                    Order_Recalculation(iID);

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult DeleteFood(int iID, int iNum)
        {
            try
            {
                using (var context = new CanteenContext())
                {
                    var orders = context.OrderDets.Where(x => x.OrderID.Equals(iID)).Where(x => x.OrderDetID.Equals(iNum)).First();
                    orders.Status = "D";

                    Order_Recalculation(iID);

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
        public JsonResult NewRecOrderOrder()
        {
            int iID = pConvInt(Request.QueryString["oID"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    int orderDets;
                    if (context.OrderDets.Where(x => x.OrderID.Equals(iID)).Any())
                    {
                        orderDets = context.OrderDets.Where(x => x.OrderID.Equals(iID)).OrderByDescending(x => x.OrderDetID).Select(x => x.OrderDetID).First() + 1;
                    }
                    else
                    {
                        orderDets = 1;
                    }

                    return Json(orderDets);
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }
    }
}