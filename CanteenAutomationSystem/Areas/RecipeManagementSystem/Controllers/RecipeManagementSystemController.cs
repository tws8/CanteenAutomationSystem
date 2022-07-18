using CanteenAutomationSystem.Areas.RecipeManagementSystem.Models;
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

namespace CanteenAutomationSystem.Areas.RecipeManagementSystem.Controllers
{
    [Authorize]
    public class RecipeManagementSystemController : BaseController
    {
        [Authorize]
        public ActionResult PurchaseOrderListing()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            int iID = pConvInt(Request.QueryString["PurchaseOrderID"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    
                    if (context.PurchaseOrders.Where(x => x.PurchaseOrderID.Equals(iID)).Where(x => x.Status.Equals("A")).Any())
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
        public ActionResult PurchaseOrderJS()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            Models.PurchaseOrder iniTBL = new Models.PurchaseOrder() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);
            TempData["TYPE"] = pRTIN(Request.QueryString["type"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    
                    if (Request.QueryString["PurchaseOrderID"] != null)
                    {
                        int iID = pConvInt(Request.QueryString["PurchaseOrderID"]);
                        var qPurchaseOrderHeader = context.PurchaseOrders.Where(x => x.PurchaseOrderID.Equals(iID));

                        iniTBL.ID = qPurchaseOrderHeader.Select(x => x.PurchaseOrderID).First();
                        iniTBL.VENDOR = qPurchaseOrderHeader.Select(x => x.Vendor).First();
                        iniTBL.VENDORNAME = context.Vendors.Where(y => y.VendorID.Equals(qPurchaseOrderHeader.Select(x => x.Vendor).FirstOrDefault())).Select(y => y.VendorName).First();
                        iniTBL.TOTPRICE = qPurchaseOrderHeader.Select(x => x.TotalPrice).First();
                        if (qPurchaseOrderHeader.Select(x => x.OrderedDateTime).First() != null)
                        {
                            iniTBL.DTORDDATETIME = (DateTime)qPurchaseOrderHeader.Select(x => x.OrderedDateTime).First();
                        }
                        if (qPurchaseOrderHeader.Select(x => x.VerifiedDateTime).First() != null)
                        {
                            iniTBL.DTVERATETIME = (DateTime)qPurchaseOrderHeader.Select(x => x.VerifiedDateTime).First();
                        }
                        if (qPurchaseOrderHeader.Select(x => x.ReceivingDateTime).First() != null)
                        {
                            iniTBL.DTRECDATETIME = (DateTime)qPurchaseOrderHeader.Select(x => x.ReceivingDateTime).First();
                        }
                        iniTBL.STATUS = qPurchaseOrderHeader.Select(x => x.Status).First();
                    }
                    else
                    {
                        var qPurchaseOrderHeader = context.PurchaseOrders.OrderByDescending(x => x.PurchaseOrderID);

                        if (qPurchaseOrderHeader.Any())
                        {
                            iniTBL.ID = qPurchaseOrderHeader.Select(x => x.PurchaseOrderID).First() + 1;
                        }
                        else
                        {
                            iniTBL.ID = 1;
                            iniTBL.VENDOR = null;
                            iniTBL.VENDORNAME = "";
                        }
                    }
                }
            }
            finally
            {
            }

            return View(iniTBL);
        }

        [Authorize]
        public ActionResult VendorListing()
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
        public ActionResult VendorData()
        {
            //------------------------------
            iniTempData();  //Reset TempData
            //------------------------------

            Models.Vendor iniTBL = new Models.Vendor() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);
            TempData["TYPE"] = pRTIN(Request.QueryString["type"]);

            int iID = pConvInt(Request.QueryString["VendorID"]);

            try
            {
                if (!string.IsNullOrEmpty(TempData["TYPE"].ToString()) && !TempData["TYPE"].ToString().Equals("N"))
                {
                    using (var context = new CanteenContext())
                    {
                        var qVendor = context.Vendors.Where(x => x.VendorID.Equals(iID)).First();

                        iniTBL.ID = qVendor.VendorID;
                        iniTBL.NAME = qVendor.VendorName;
                        iniTBL.PHONE = qVendor.VendorContact;
                        iniTBL.EMAIL = qVendor.VendorEmail;
                    }
                }
                else
                {
                    using (var context = new CanteenContext())
                    {
                        if (context.Vendors.Any())
                        {
                            iniTBL.ID = context.Vendors.OrderByDescending(x => x.VendorID).Select(x => x.VendorID).First() + 1;
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
        public JsonResult PurchaseOrderContent()
        {
            int iID = pConvInt(Request.QueryString["iID"]);
            int iNUM = pConvInt(Request.QueryString["iNUM"]);

            Models.PurchaseOrderDet iniTBL = new Models.PurchaseOrderDet() { };

            try
            {
                using(var context = new CanteenContext())
                {
                    var qIngredientOrder = context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID)).Where(x => x.PurchaseOrderDetID.Equals(iNUM)).Select(x => x).First();

                    iniTBL.ID = iID;
                    iniTBL.PURCHASEORDERDETID = iNUM;
                    iniTBL.INGREDIENTID = qIngredientOrder.IngredientID;
                    iniTBL.INGREDIENTDESC = context.Ingredients.Where(x => x.IngredientID.Equals(qIngredientOrder.IngredientID)).First().IngredientDesc;
                    iniTBL.UNITPRICE = qIngredientOrder.UnitPrice;
                    iniTBL.QUANTITY = qIngredientOrder.Quantity;
                    iniTBL.TOTPRICE = qIngredientOrder.IngredientPrice;
                    iniTBL.STATUS = qIngredientOrder.Status;

                }
            }
            finally
            {
            }
            return Json(iniTBL, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult _VENDOR(string sVendor)
        {
            int iVendor = pConvInt(sVendor);
            TempDTO iniTBL = new TempDTO() { };

            try
            {
                using (var context = new CanteenContext())
                {
                    if (context.Vendors.Where(x => x.VendorID.Equals(iVendor)).Any())
                    {
                        var qVendor = context.Vendors.Where(x => x.VendorID.Equals(iVendor)).Select(x => x).First();

                        iniTBL.str1 = qVendor.VendorName;
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
        public JsonResult _INGREDIENT(string sIngredientID)
        {
            int iIngredientID = pConvInt(sIngredientID);
            TempDTO iniTBL = new TempDTO() { };

            try
            {
                using(var context = new CanteenContext())
                {
                    if (context.Ingredients.Where(x => x.IngredientID.Equals(iIngredientID)).Any())
                    {
                        var qIngredient = context.Ingredients.Where(x => x.IngredientID.Equals(iIngredientID)).Select(x => x).First();

                        iniTBL.str1 = qIngredient.IngredientDesc;
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
            int iIngredientID = pConvInt(Request.QueryString["iID"]);
            int iQty = pConvInt(Request.QueryString["iQty"]);
            decimal dcPrice = pConvInt(Request.QueryString["dcPrice"]);

            TempDTO iniTBL = new TempDTO() { };

            try
            {
                iniTBL.dec1 = (dcPrice * pConvDec(iQty));
            }
            finally
            {
            }

            return Json(iniTBL, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddPurchaseOrder_Chk(Models.PurchaseOrder Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);
                int iVENDOR = pConvInt(Order.VENDOR);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (context.PurchaseOrders.Where(x => x.PurchaseOrderID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID used"));
                        }

                        if (!context.Vendors.Where(x => x.VendorID.Equals(iVENDOR)).Any())
                        {
                            return Json(Ermsg("Invalid Vendor"));
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
        public JsonResult UpdatePurchaseOrder_Chk(Models.PurchaseOrder Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);
                int iVENDOR = pConvInt(Order.VENDOR);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.PurchaseOrders.Where(x => x.Status.Equals("A")).Where(x => x.PurchaseOrderID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("ID not found or Purchase Order not allowed to update"));
                        }

                        if (!context.Vendors.Where(x => x.VendorID.Equals(iVENDOR)).Any())
                        {
                            return Json(Ermsg("Invalid Vendor"));
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
        public JsonResult VerifyPurchaseOrder_Chk(Models.PurchaseOrder Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.PurchaseOrders.Where(x => x.Status.Equals("R")).Where(x => x.PurchaseOrderID.Equals(iID)).Any())
                        {
                            return Json(Ermsg("Purchase Order not allowed to approve or reject"));
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
        public JsonResult ReceivePurchaseOrder_Chk(Models.PurchaseOrder Order)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Order.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.PurchaseOrders.Where(x => x.Status.Equals("S")).Where(x => x.PurchaseOrderID.Equals(iID)).Any())
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
                    
                    if (!context.PurchaseOrders.Where(x => x.PurchaseOrderID.Equals(iID)).Any())
                    {
                        return Json(Ermsg("ID not found or ID belongs to other Customer"));
                    }

                    if (context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID)).Where(x => x.PurchaseOrderDetID.Equals(iNum)).Any())
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
                    
                    if (!context.PurchaseOrders.Where(x => x.PurchaseOrderID.Equals(iID)).Any())
                    {
                        return Json(Ermsg("ID not found or ID belongs to other Customer"));
                    }

                    if (!context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID)).Where(x => x.PurchaseOrderDetID.Equals(iNum)).Any())
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
        public JsonResult AddVendor_Chk(Models.Vendor Vendor)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Vendor.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (context.Vendors.Where(x => x.VendorID.Equals(iID)).Any())
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
        public JsonResult UpdateVendor_Chk(Models.Vendor Vendor)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                int iID = pConvInt(Vendor.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.Vendors.Where(x => x.VendorID.Equals(iID)).Any())
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
        public JsonResult AddPurchaseOrder(Models.PurchaseOrder Order)
        {
            int iID = pConvInt(Order.ID);
            int iVENDOR = pConvInt(Order.VENDOR);

            try
            {
                using (var context = new CanteenContext())
                {
                    var purchaseOrders = new CanteenAutomationSystem.Models.PurchaseOrder()
                    {
                        PurchaseOrderID = iID,
                        Vendor = iVENDOR,
                        TotalPrice = 0,
                        Status = "A",
                    };
                    context.PurchaseOrders.Add(purchaseOrders);
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
        public JsonResult UpdatePurchaseOrder(Models.PurchaseOrder Order)
        {
            int iID = pConvInt(Order.ID);
            int iVENDOR = pConvInt(Order.VENDOR);

            try
            {
                using (var context = new CanteenContext())
                {
                    var purchaseOrders = context.PurchaseOrders.First(x => x.PurchaseOrderID.Equals(iID));
                    purchaseOrders.Vendor = iVENDOR;
                    context.SaveChanges();

                    PurchaseOrder_Recalculation(iID);

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult RequestPurchaseOrder(Models.PurchaseOrder Order)
        {
            int iID = pConvInt(Order.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var purchaseOrders = context.PurchaseOrders.First(x => x.PurchaseOrderID.Equals(iID));
                    purchaseOrders.Status = "R";
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
        public JsonResult ApprovePurchaseOrder(Models.PurchaseOrder Order)
        {
            int iID = pConvInt(Order.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    DateTime dtVerify = DateTime.Now;
                    var purchaseOrders = context.PurchaseOrders.First(x => x.PurchaseOrderID.Equals(iID));
                    purchaseOrders.VerifiedDateTime = dtVerify;
                    purchaseOrders.Status = "S";
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
        public JsonResult RejectPurchaseOrder(Models.PurchaseOrder Order)
        {
            int iID = pConvInt(Order.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    DateTime dtVerify = DateTime.Now;
                    var purchaseOrders = context.PurchaseOrders.First(x => x.PurchaseOrderID.Equals(iID));
                    purchaseOrders.VerifiedDateTime = dtVerify;
                    purchaseOrders.Status = "P";
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
        public JsonResult ReceivePurchaseOrder(Models.PurchaseOrder Order)
        {
            int iID = pConvInt(Order.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var orderDets = context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID));
                    foreach (var orderDet in orderDets.ToList())
                    {
                        var ingredients = context.Ingredients.Where(x => x.IngredientID.Equals(orderDet.IngredientID)).First();
                        ingredients.IngredientQty += orderDet.Quantity;
                        context.SaveChanges();
                    }

                    DateTime dtNow = DateTime.Now;
                    var purchaseOrders = context.PurchaseOrders.First(x => x.PurchaseOrderID.Equals(iID));
                    purchaseOrders.ReceivingDateTime = dtNow;
                    purchaseOrders.Status = "C";
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
        public JsonResult DeletePurchaseOrder(Models.PurchaseOrder Order)
        {
            int iID = pConvInt(Order.ID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var orderDets = context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID));
                    foreach(var orderDet in orderDets)
                    {
                        orderDet.Status = "D";
                    }
                    context.SaveChanges();

                    var orders = context.PurchaseOrders.First(x => x.PurchaseOrderID.Equals(iID));
                    orders.TotalPrice = 0;
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
        public JsonResult AddIngredient(Models.PurchaseOrderDet OrderDet)
        {
            int iID = pConvInt(Request.QueryString["iID"].ToString());
            int iNum = pConvInt(OrderDet.PURCHASEORDERDETID);
            int iSubID = pConvInt(OrderDet.INGREDIENTID);
            int iQty = pConvInt(OrderDet.QUANTITY);
            decimal dcUnitPrice = pConvDec(OrderDet.UNITPRICE);
            decimal dcTotPrice;

            try
            {
                using (var context = new CanteenContext())
                {
                    dcTotPrice = pConvDec(iQty) * dcUnitPrice;
                    var orderDets = new CanteenAutomationSystem.Models.PurchaseOrderDet
                    {
                        PurchaseOrderID = iID,
                        PurchaseOrderDetID = iNum,
                        IngredientID = iSubID,
                        UnitPrice = dcUnitPrice,
                        Quantity = iQty,
                        IngredientPrice = dcTotPrice,
                        Status = "A",
                    };
                    context.PurchaseOrderDets.Add(orderDets);
                    context.SaveChanges();

                    PurchaseOrder_Recalculation(iID);

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult UpdateIngredient(Models.PurchaseOrderDet OrderDet)
        {
            int iID = pConvInt(OrderDet.ID);
            int iNum = pConvInt(OrderDet.PURCHASEORDERDETID);
            int iSubID = pConvInt(OrderDet.INGREDIENTID);
            int iQty = pConvInt(OrderDet.QUANTITY);
            decimal dcUnitPrice = pConvDec(OrderDet.UNITPRICE);
            decimal dcTotPrice;

            try
            {
                using (var context = new CanteenContext())
                {
                    dcTotPrice = pConvDec(iQty) * dcUnitPrice;

                    var orders = context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID)).Where(x => x.PurchaseOrderDetID.Equals(iNum)).First();
                    orders.IngredientID = iSubID;
                    orders.UnitPrice = dcUnitPrice;
                    orders.Quantity = iQty;
                    orders.IngredientPrice = dcTotPrice;
                    context.SaveChanges();

                    PurchaseOrder_Recalculation(iID);

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
                    var orders = context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID)).Where(x => x.PurchaseOrderDetID.Equals(iNum)).First();
                    orders.Status = "D";
                    context.SaveChanges();

                    PurchaseOrder_Recalculation(iID);

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult AddVendor(Models.Vendor Vendor)
        {
            int iID = pConvInt(Vendor.ID);
            string sNAME = pRTIN(Vendor.NAME);
            string sPHONE = pRTIN(Vendor.PHONE);
            string sEMAIL = pRTIN(Vendor.EMAIL);

            try
            {
                using (var context = new CanteenContext())
                {
                    var vendors = new CanteenAutomationSystem.Models.Vendor()
                    {
                        VendorID = iID,
                        VendorName = sNAME,
                        VendorContact = sPHONE,
                        VendorEmail = sEMAIL,

                    };
                    context.Vendors.Add(vendors);
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
        public JsonResult UpdateVendor(Models.Vendor Vendor)
        {
            int iID = pConvInt(Vendor.ID);
            string sNAME = pRTIN(Vendor.NAME);
            string sPHONE = pRTIN(Vendor.PHONE);
            string sEMAIL = pRTIN(Vendor.EMAIL);

            try
            {
                using (var context = new CanteenContext())
                {
                    var vendors = context.Vendors.First(x => x.VendorID.Equals(iID));
                    vendors.VendorName = sNAME;
                    vendors.VendorContact = sPHONE;
                    vendors.VendorEmail = sEMAIL;
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
                    if (context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID)).Any())
                    {
                        orderDets = context.PurchaseOrderDets.Where(x => x.PurchaseOrderID.Equals(iID)).OrderByDescending(x => x.PurchaseOrderDetID).Select(x => x.PurchaseOrderDetID).First() + 1;
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