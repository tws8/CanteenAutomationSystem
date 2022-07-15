using System;
using System.Data.Odbc;
using System.Web;
using CanteenAutomationSystem.Variables;
using static CanteenAutomationSystem.Include.Proc;

namespace CanteenAutomationSystem.Include
{
    public class Startup
    {
        public static DateTime gDate;
        public static string gCompanyName = "";                             //Company Name
        public static string gSystemName = "Canteen Automation System";     //System Name
        public static string gSystemName_Short = "CAS";                     //System Name Short

        public static void iniVariable()
        {
            gSetting gSetting = new gSetting() { };

            //Assign value to Client Variable
            HttpContext.Current.Session["gSetting"] = gSetting;
        }
    }
    
}