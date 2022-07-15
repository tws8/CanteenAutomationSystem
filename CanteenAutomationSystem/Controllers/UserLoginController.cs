using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CanteenAutomationSystem.DAL;
using CanteenAutomationSystem.Models;
using CanteenAutomationSystem.Variables;
using static CanteenAutomationSystem.Include.Proc;

namespace CanteenAutomationSystem.Controllers
{
    public class UserLoginController : ShareBaseController
    {
        public object CookieAuthenticationDefaults { get; private set; }

        // GET: UserLogin
        public ActionResult Login()
        {
            return View(new User());
        }

        public ActionResult Register()
        {
            return View(new NewUser());
        }

        //Login POST
        [HttpPost]
        public ActionResult Login(User user)
        {
            string message = "";

            string sUserId = user.UserID;
            string sUnEncryptedUserPw = user.UserPW;
            string sUserType = user.UserType;
            byte[] sEncryptedUserPw;

            if (ModelState.IsValid)
            {
                try
                {
                    //Clear All Cookies
                    List<string> names = new List<string>();
                    foreach (var cookieKey in Request.Cookies.Keys)
                    {
                        names.Add(cookieKey.ToString());
                    }
                    foreach (var name in names)
                    {
                        var cookie = new HttpCookie(name);
                        cookie.Expires = DateTime.Now.AddDays(-7);
                        Response.Cookies.Add(cookie);

                    }

                    using (var context = new CanteenContext())
                    {
                        var encryptedUserPw = EncryptPlainTextToCipherText(pRTINU(sUnEncryptedUserPw));
                        if (sUserType.Equals("0"))
                        {
                            var qIsCustUser = context.Customers.Where(x => x.CustID.Equals(sUserId)).Where(x => x.CustPW.Equals(encryptedUserPw));

                            if (qIsCustUser.Any())
                            {
                                /*
                                byte[] encData_byte = new byte[UserPw.Length];
                                encData_byte = Encoding.UTF8.GetBytes(UserPw);
                                UserPw = Convert.ToBase64String(encData_byte);
                                */

                                //byte[] empty;
                                //string sEmpty = "";
                                //AesEncryption(true, sUnEncryptedUserPw, null, out sEncryptedUserPw, out sEmpty);
                                //AesEncryption(false, null, sEncryptedUserPw, out empty, out sUnEncryptedUserPw);
                                //query = query.Where(x => x.UserPassword.Equals(sEncryptedUserPw));

                                Session["_USERNAME"] = sUserId.ToUpper();
                                Session["_USERTYPE"] = "C";

                                //Set Cookie Login done
                                FormsAuthentication.SetAuthCookie(sUserId.ToUpper(), false);

                                //Pass User Name to Other Modules
                                HttpCookie USER = new HttpCookie("USER");
                                USER.Value = sUserId.ToUpper();
                                Response.Cookies.Add(USER);

                                HttpCookie TYPE = new HttpCookie("TYPE");
                                TYPE.Value = "C";
                                Response.Cookies.Add(TYPE);

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                message = "Invalid ID or Password";
                            }
                        }
                        else
                        {
                            var qIsBizUser = context.BizUsers.Where(x => x.BizUserID.Equals(sUserId)).Where(x => x.BizUserPW.Equals(encryptedUserPw));

                            if (qIsBizUser.Any())
                            {
                                Session["_USERNAME"] = sUserId.ToUpper();
                                Session["_USERTYPE"] = qIsBizUser.FirstOrDefault().BizDeptID;

                                //Set Cookie Login done
                                FormsAuthentication.SetAuthCookie(sUserId.ToUpper(), false);

                                //Pass User Name to Other Modules
                                HttpCookie USER = new HttpCookie("USER");
                                USER.Value = sUserId.ToUpper();
                                Response.Cookies.Add(USER);

                                HttpCookie TYPE = new HttpCookie("TYPE");
                                TYPE.Value = qIsBizUser.FirstOrDefault().BizDeptID;
                                Response.Cookies.Add(TYPE);

                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                message = "Invalid ID or Password";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            ViewBag.Message = message;
            return View(user);
        }

        //Login POST
        [HttpPost]
        public ActionResult Register(NewUser NewUser)
        {
            string sID = NewUser.ID;
            string sPassword = NewUser.PASSWORD;
            string sName = NewUser.NAME;
            string sStatus = NewUser.STATUS;
            string sTel = NewUser.TEL;
            string sEmail = NewUser.EMAIL;

            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new CanteenContext())
                    {
                        var customers = new Customer()
                        {
                            CustID = sID,
                            CustName = sName,
                            CustPW = EncryptPlainTextToCipherText(pRTINU(sPassword)),
                            CustMemberStatus = sStatus,
                            CustPhone = sTel,
                            CustEmail = sEmail,

                        };
                        context.Customers.Add(customers);

                        context.SaveChanges();
                    }

                    ViewBag.Message = "New user registered";
                    return View(new NewUser());
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            else
            {
                message = "ID used";
            }
            ViewBag.Message = message;
            return View(new NewUser());
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            List<string> names = new List<string>();
            foreach (var cookieKey in Request.Cookies.Keys)
            {
                names.Add(cookieKey.ToString());
            }
            foreach (var name in names)
            {
                var cookie = new HttpCookie(name);
                cookie.Expires = DateTime.Now.AddDays(-7);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login", "UserLogin");
        }

        [HttpPost]
        public JsonResult _Authz()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json("Time Out");
            }
            return Json("OK");
            // User Timeout Status
        }

    }
}
