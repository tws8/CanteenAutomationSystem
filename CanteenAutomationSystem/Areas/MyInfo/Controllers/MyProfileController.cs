using CanteenAutomationSystem.Areas.MyInfo.Models;
using CanteenAutomationSystem.Controllers;
using CanteenAutomationSystem.DAL;
using CanteenAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Linq;
using System.Web.Mvc;
using static CanteenAutomationSystem.Include.Proc;

namespace CanteenAutomationSystem.Areas.MyInfo.Controllers
{
    [Authorize]
    public class MyProfileController : BaseController
    {
        [Authorize]
        public ActionResult MyProfile()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            MyProfile iniTBL = new MyProfile() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    if (pConvStr(Session["_USERTYPE"]).Equals("C")) // Customer
                    {
                        var qMyProfile = context.Customers.Where(x => x.CustID.Equals(sUserName));

                        iniTBL.ID = qMyProfile.Select(x => x.CustID).FirstOrDefault();
                        iniTBL.NAME = qMyProfile.Select(x => x.CustName).FirstOrDefault();
                        iniTBL.TEL = qMyProfile.Select(x => x.CustPhone).FirstOrDefault();
                        iniTBL.EMAIL = qMyProfile.Select(x => x.CustEmail).FirstOrDefault();
                        iniTBL.STATUS = qMyProfile.Select(x => x.CustMemberStatus).FirstOrDefault();
                        iniTBL.CREDITBAL = qMyProfile.Select(x => x.BalCredit).FirstOrDefault();
                    }
                    else
                    {
                        var qMyProfile = context.BizUsers.Where(x => x.BizUserID.Equals(sUserName));

                        iniTBL.ID = qMyProfile.Select(x => x.BizDeptID).FirstOrDefault();
                        iniTBL.NAME = qMyProfile.Select(x => x.BizUserName).FirstOrDefault();

                        IDictionary<string, string> dictDept = new Dictionary<string, string>();

                        TempData["MYDEPT"] = context.BizUsers.Where(x => x.BizUserID.Equals(sUserName)).Select(x => x.BizDeptID).FirstOrDefault();

                        foreach (var sDept in context.BizDepts)
                        {
                            dictDept.Add(sDept.BizDeptID, sDept.BizDeptDesc);
                        }

                        ViewData["DEPT"] = dictDept;
                    }
                }
            }
            finally
            {
            }
            return View(iniTBL);
        }

        [Authorize]
        public ActionResult MyPw()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            MyPw iniTBL = new MyPw() { };
            return View(iniTBL);
        }

        [Authorize]
        public ActionResult MyNewBizUser()
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            MyProfile iniTBL = new MyProfile() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            IDictionary<string, string> dictDept = new Dictionary<string, string>();

            try
            {
                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    TempData["MYDEPT"] = context.BizUsers.Where(x => x.BizUserID.Equals(sUserName)).Select(x => x.BizDeptID).FirstOrDefault();

                    foreach (var sDept in context.BizDepts)
                    {
                        dictDept.Add(sDept.BizDeptID, sDept.BizDeptDesc);
                    }
                }
            }
            finally
            {
            }

            ViewData["DEPT"] = dictDept;
            return View(iniTBL);
        }

        [Authorize]
        public ActionResult MyCustomerListing()
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
        public ActionResult MyCustomerData()
        {
            //------------------------------
            iniTempData();  //Reset TempData
            //------------------------------

            MyCreditBal iniTBL = new MyCreditBal() { };
            TempData["PAGECONT"] = pConvInt(Request.QueryString["page"]);
            TempData["SEARCHCONT"] = pRTIN(Request.QueryString["search"]);

            string sID = pRTIN(Request.QueryString["CustomerID"]);

            try
            {
                using (var context = new CanteenContext())
                {
                    var qCustomer = context.Customers.Where(x => x.CustID.Equals(sID)).First();

                    iniTBL.CUSTOMERID = qCustomer.CustID;
                    iniTBL.CREDITBAL = 0;
                }
            }
            finally
            {
            }
            return View(iniTBL);
        }

        [HttpPost]
        public JsonResult MyPw_Chk(MyPw MyPw)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                string sPASSWORD = pRTIN(MyPw.PASSWORD);
                string sCPASSWORD = pRTIN(MyPw.CPASSWORD);

                try
                {
                    if (!sPASSWORD.Equals(sCPASSWORD))
                    {
                        return Json(Ermsg("Password Not Match."));
                    }
                    return Json("OK");
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
        public JsonResult MyNewBizUser_Chk(MyProfile MyProfile)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                string sID = pRTINU(MyProfile.ID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (context.BizUsers.Where(x => x.BizUserID.Equals(sID)).Any())
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
        public JsonResult UpdateUser_Chk(MyCreditBal MyCreditBal)
        {
            IEnumerable<ModelError> allErrors;

            if (ModelState.IsValid)
            {
                string sID = pRTIN(MyCreditBal.CUSTOMERID);

                try
                {
                    using (var context = new CanteenContext())
                    {
                        if (!context.Customers.Where(x => x.CustID.Equals(sID)).Any())
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
        public JsonResult MyProfile(MyProfile MyProfile)
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            string sID = pRTINU(MyProfile.ID);
            string sNAME = pRTIN(MyProfile.NAME);

            try
            {
                MyProfile iniTBL = new MyProfile() { };

                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    if (pConvStr(Session["_USERTYPE"]).Equals("C")) // Customer
                    {
                        string sTEL = pRTIN(MyProfile.TEL);
                        string sEMAIL = pRTIN(MyProfile.EMAIL);

                        var qMyProfile = context.Customers.Where(x => x.CustID.Equals(sUserName)).First();

                        qMyProfile.CustName = sNAME;
                        qMyProfile.CustPhone = sTEL;
                        qMyProfile.CustEmail = sEMAIL;
                        context.SaveChanges();
                    }
                    else
                    {
                        var qMyProfile = context.BizUsers.Where(x => x.BizUserID.Equals(Session["_USERNAME"])).First();
                        qMyProfile.BizUserName = sNAME;

                    }

                    return Json("OK");
                }

                
            }
            catch(Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

        [HttpPost]
        public JsonResult MyPw(MyPw MyPw)
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            string sPASSWORD = EncryptPlainTextToCipherText(pRTINU(MyPw.PASSWORD));

            try
            {
                MyPw iniTBL = new MyPw() { };

                using (var context = new CanteenContext())
                {
                    string sUserName = pConvStr(Session["_USERNAME"]);
                    if (pConvStr(Session["_USERTYPE"]).Equals("C")) // Customer
                    {
                        var qMyProfile = context.Customers.Where(x => x.CustID.Equals(sUserName)).First();

                        qMyProfile.CustPW = sPASSWORD;
                        context.SaveChanges();
                    }
                    else
                    {
                        var qMyProfile = context.BizUsers.Where(x => x.BizUserID.Equals(sUserName)).First();

                        qMyProfile.BizUserPW = sPASSWORD;
                        context.SaveChanges();
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
        public JsonResult MyNewBizUser(MyProfile MyProfile)
        {
            //------------------------------
            iniTempData();  //Reset TempDate
            //------------------------------

            string sID = pRTINU(MyProfile.ID);
            string sPASSWORD = MyProfile.ID;
            string sNAME = pRTIN(MyProfile.NAME);
            string sTEL = pRTIN(MyProfile.TEL);
            string sEMAIL = pRTIN(MyProfile.EMAIL);
            string sDEPT = pRTINU(MyProfile.DEPT);

            try
            {
                using (var context = new CanteenContext())
                {
                    var bizUsers = new BizUser()
                    {
                        BizUserID = sID,
                        BizUserPW = EncryptPlainTextToCipherText(sPASSWORD),
                        BizUserName = sNAME,
                        BizDeptID = sDEPT

                    };
                    context.BizUsers.Add(bizUsers);

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
        public JsonResult LoadCreditBal(MyCreditBal MyCreditBal)
        {
            string sID = pRTIN(MyCreditBal.CUSTOMERID);
            decimal dcCREDITBAL = pConvDec(MyCreditBal.CREDITBAL);

            try
            {
                using (var context = new CanteenContext())
                {
                    var customers = context.Customers.First(x => x.CustID.Equals(sID));
                    customers.BalCredit = customers.BalCredit + dcCREDITBAL;
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
        public JsonResult RegisterMembership(MyCreditBal MyCreditBal)
        {
            string sID = pRTIN(MyCreditBal.CUSTOMERID);

            try
            {
                using (var context = new CanteenContext())
                {
                    var customers = context.Customers.First(x => x.CustID.Equals(sID));
                    customers.CustMemberStatus = "Y";
                    context.SaveChanges();

                    return Json("OK");
                }
            }
            catch (Exception ex)
            {
                return Json(Ermsg(ex.Message));
            }
        }

    }
}