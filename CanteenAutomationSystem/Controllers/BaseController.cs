using CanteenAutomationSystem.DAL;
using CanteenAutomationSystem.Models;
using CanteenAutomationSystem.Variables;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using static CanteenAutomationSystem.Include.Proc;

namespace CanteenAutomationSystem.Controllers
{
    [Authorize]
    public class BaseController : ShareBaseController
    {
        gSetting gSetting;

        int pPageSize;
        int pPage;
        string pFldName;
        string pType;
        string pContent;
        string pModal;
        string pSearch;

        int startIndex = 0;
        int currentIndex = 0;
        double dTotalItem, dTotalPage;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var gSetting = (gSetting)Session["gSetting"];

            TempData["gCURR"] = gSetting.gCurr;
            TempData["gDEC"] = gSetting.gDec;
            TempData["gDISCOUNTRATE"] = gSetting.gDiscountRate;

            TempData["gFMPAGESIZE"] = gSetting.gFmPageSize;
            TempData["gVWPAGESIZE"] = gSetting.gVwPageSize;
            return ;
        }

        public IEnumerable<ModelError> Ermsg(string msg)
        {
            IEnumerable<ModelError> allErrors;
            
            if (!string.IsNullOrEmpty(msg))
            {
                ModelState.Clear();
                ModelState.AddModelError("Error", msg);
            }

            allErrors = ModelState.Values.SelectMany(v => v.Errors);
            return allErrors;
        }

        public DataSet IniVwSrcListing(object oPageSize, object oPage, object oFldName, object oType, object oContent, object oModal, object oSearch, object oTable)
        {
            //------------------------------
            iniTempData();  //Reset TempData
            //------------------------------

            pPageSize = pConvInt(oPageSize);
            pPage = pConvInt(oPage);
            pFldName = pConvStr(oFldName);
            pType = pConvStr(oType);
            pContent = pConvStr(oContent);
            pModal = pConvStr(oModal);
            pSearch = pConvStr(oSearch);

            startIndex = 0;
            currentIndex = 0;

            ViewData["fldName"] = pConvStr(pFldName);
            ViewData["pType"] = pConvStr(pType);
            ViewData["pContent"] = pConvStr(pContent);
            ViewData["pModal"] = pConvStr(pModal);

            TempData["PAGE"] = pConvInt(pPage);
            TempData["SEARCH"] = pConvStr(pSearch);

            if (pPage > 1)
            {
                currentIndex = (pPageSize * (pPage - 1));
            }
            else
            {
                currentIndex = startIndex;
            }


            DataSet ds = new DataSet();
            ds.Tables.Add((DataTable)oTable);

            if (dTotalItem <= pPageSize)
            {
                TempData["TOTPAGE"] = 1;

            }
            else if ((dTotalItem) % pPageSize == 0)
            {

                TempData["TOTPAGE"] = pConvInt((dTotalItem - 1) / pPageSize);
            }
            else
            {
                dTotalPage = ((dTotalItem) / pPageSize);
                if (dTotalPage > pConvInt(dTotalPage))
                {
                    TempData["TOTPAGE"] = pConvInt(dTotalPage) + 1;
                }
                else
                {
                    TempData["TOTPAGE"] = pConvInt(dTotalPage);
                }
            }

            if (dTotalItem > pPageSize)
            {
                TempData["TOTITEM"] = dTotalItem;
            }
            else
            {
                TempData["TOTITEM"] = dTotalItem;
            }
            return ds;
        }

        public DataSet IniVwListing(object oPageSize, object oPage, object oSearch, object oTable)
        {
            //------------------------------
            iniTempData();  //Reset TempData
            //------------------------------

            pPageSize = pConvInt(oPageSize);
            pPage = pConvInt(oPage);
            pSearch = pConvStr(oSearch);

            startIndex = 0;
            currentIndex = 0;

            //Pass to View
            TempData["PAGE"] = pPage;
            TempData["SEARCH"] = pSearch;

            if (pPage > 1)
            {
                currentIndex = (pPageSize * (pPage - 1));
            }
            else
            {
                currentIndex = startIndex;
            }

            DataSet ds = new DataSet();
            ds.Tables.Add((DataTable)oTable);

            if (pPageSize > 0)
            {
                if (dTotalItem <= pPageSize)
                {
                    TempData["TOTPAGE"] = 1;
                }
                else if ((dTotalItem) % pPageSize == 0)
                {
                    TempData["TOTPAGE"] = pConvInt((dTotalItem - 1) / pPageSize);
                }
                else
                {
                    dTotalPage = ((dTotalItem) / pPageSize);
                    if (dTotalPage > pConvInt(dTotalPage))
                    {
                        TempData["TOTPAGE"] = pConvInt(dTotalPage) + 1;
                    }
                    else
                    {
                        TempData["TOTPAGE"] = pConvInt(dTotalPage);
                    }
                }
                if (dTotalItem > pPageSize)
                {
                    TempData["TOTITEM"] = dTotalItem;
                }
                else
                {
                    TempData["TOTITEM"] = dTotalItem;
                }
            }
            return ds;
        }

        public void Order_Recalculation(int iOrder)
        {
            using (var context = new CanteenContext())
            {
                gSetting = new gSetting();
                string sUserName = pConvStr(Session["_USERNAME"]);
                decimal dcTotPrice, dcDiscount, dcActPrice;

                var orders = context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iOrder)).First();

                if (context.OrderDets.Where(x => x.OrderID.Equals(iOrder)).Where(x => !x.Status.Equals("D")).Any())
                {
                    dcTotPrice = context.OrderDets.Where(x => x.OrderID.Equals(iOrder)).Where(x => !x.Status.Equals("D")).Sum(x => x.TotalPrice);

                    if (context.Customers.Where(x => x.CustID.Equals(sUserName)).Select(x => x.CustMemberStatus).First().Equals("Y"))
                    {
                        dcDiscount = (dcTotPrice * gSetting.gDiscountRate) / 100;
                        dcActPrice = dcTotPrice - dcDiscount;
                    }
                    else
                    {
                        dcDiscount = 0;
                        dcActPrice = dcTotPrice;
                    }

                    orders.TotalPrice = dcTotPrice;
                    orders.Discount = dcDiscount;
                    orders.ActPrice = dcActPrice;
                    if (dcTotPrice.Equals(0))
                    {
                        orders.Status = "D";
                    }
                }
                else
                {
                    orders.TotalPrice = 0;
                    orders.Discount = 0;
                    orders.ActPrice = 0;
                    orders.Status = "D";
                }
                context.SaveChanges();
            }
        }

        public void Order_Reconfirmation()
        {
            using (var context = new CanteenContext())
            {
                string sMsg = "Quantity of ";
                bool bFirstMsg = true;
                string sUserName = pConvStr(Session["_USERNAME"]);
                int iActOrder = context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.Status.Equals("A")).Select(x => x.OrderID).First();
                List<OrderDet> lstOrderDet = context.OrderDets.Where(y => y.OrderID.Equals(iActOrder)).ToList();

                if (lstOrderDet.Count > 0)
                {
                    foreach (var orderDet in lstOrderDet)
                    {
                        var qFood = context.Foods.Where(x => x.FoodID.Equals(orderDet.FoodID)).Where(x => x.Status.Equals("I"));

                        if (qFood.Any())
                        {
                            var foods = qFood.First();
                            //foods.FoodCurOrdQty = 0;
                            context.SaveChanges();

                            orderDet.Quantity = 0;
                            context.SaveChanges();

                            if (!bFirstMsg)
                            {
                                sMsg += ", " + foods.FoodDesc;
                            }
                            else
                            {
                                sMsg += foods.FoodDesc;
                                bFirstMsg = false;
                            }
                        }
                    }

                    if (!bFirstMsg)
                    {
                        TempData["MSG"] = sMsg + " is reset to 0, so these foods will be removed from the order";
                    }
                    else
                    {
                        TempData["MSG"] = "";
                    }

                    var orderDets = context.OrderDets.Where(y => y.OrderID.Equals(iActOrder)).Where(y => y.Quantity.Equals(0)).Select(y => y);

                    if (orderDets.Any())
                    {
                        foreach (var orderDet in orderDets)
                        {
                            orderDet.Status = "D";
                            context.SaveChanges();
                        }
                    }

                    if (!context.OrderDets.Where(y => y.OrderID.Equals(iActOrder)).Any())
                    {
                        var orders = context.Orders.Where(y => y.Customer.Equals(sUserName)).Where(y => y.OrderID.Equals(iActOrder)).Select(y => y).First();
                        orders.Status = "D";
                        context.SaveChanges();
                    }
                }
                else
                {
                    var orders = context.Orders.Where(x => x.Customer.Equals(sUserName)).Where(x => x.OrderID.Equals(iActOrder)).First();
                    orders.Status = "D";
                    context.SaveChanges();

                    TempData["MSG"] = "";
                }
            }
        }
    }
}