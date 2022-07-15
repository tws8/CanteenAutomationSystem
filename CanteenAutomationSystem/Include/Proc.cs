using CanteenAutomationSystem.Models;
using System;
using System.Data.Odbc;
using System.Text;
using System.Web;

namespace CanteenAutomationSystem.Include
{
    public static class Proc
    {
        static OdbcConnection TempCon;
        static OdbcCommand TempCmd;
        static string sSQL;

        public static string PassConv(string Password, int iL = 3)
        {
            int iTotal = 0;

            var bytes = Encoding.ASCII.GetBytes(Password);

            for (var iLoop = 0; iLoop < bytes.Length; iLoop++)
            {
                // bytes[iLoop] = iValue
                iTotal += bytes[iLoop] * (iL + iLoop);
            }
            return iTotal.ToString();
        }
        public static bool isNumeric(string s)
        {
            return decimal.TryParse(s, out decimal n);
        }
        public static string pRTIN(object oRSField)
        {
            string sStr;
            string aRSField = Convert.ToString(oRSField);
            if (aRSField == null || aRSField == "")
            {
                return "";
            }
            else
            {
                sStr = aRSField.Replace("'", "''").Replace(@"\", @"\\").Replace(((char)34).ToString(), " ");
                return sStr;
            }

        }
        public static string pRTINU(object aRSField)
        {
            string sStr = Convert.ToString(aRSField);

            if (aRSField == null || aRSField == "")
            {
                return "";
            }
            else
            {
                sStr = aRSField.ToString().ToUpper().TrimEnd();
                sStr = sStr.Replace("'", "''").Replace(@"\", @"\\").Replace(((char)34).ToString(), " ");
                return sStr;
            }
        }
        public static string pDatetime(object oDate)
        {
            DateTime dTemp = Convert.ToDateTime(Convert.ToString(oDate));
            string sStr;
            string sMonth = dTemp.Month.ToString().PadLeft(2, '0');
            string sDay = dTemp.Day.ToString().PadLeft(2, '0');
            string sHour = dTemp.Hour.ToString().PadLeft(2, '0');
            string sMinute = dTemp.Minute.ToString().PadLeft(2, '0');
            string sSecond = dTemp.Second.ToString().PadLeft(2, '0');

            sStr = dTemp.Year + "-" + sMonth + "-" + sDay + " ";
            sStr = sStr + sHour + ":" + sMinute + ":" + sSecond;

            return sStr;
        }
        public static string pDateYMD(object oDate)
        {
            string sStr = "";

            if (!string.IsNullOrEmpty(Convert.ToString(oDate)))
            {
                DateTime dTemp = Convert.ToDateTime(oDate);
                string sMonth = dTemp.Month.ToString().PadLeft(2, '0');
                string sDay = dTemp.Day.ToString().PadLeft(2, '0');

                sStr = dTemp.Year + "-" + sMonth + "-" + sDay;
            }
            return sStr;
        }
        public static string pDateDMY(object oDate)
        {
            string sStr = "";
            if (!string.IsNullOrEmpty(Convert.ToString(oDate)))
            {
                DateTime dTemp = DateTime.Parse(Convert.ToString(oDate));

                string sMonth = dTemp.Month.ToString().PadLeft(2, '0');
                string sDay = dTemp.Day.ToString().PadLeft(2, '0');

                sStr = sDay + "/" + sMonth + "/" + dTemp.Year;
            }
            return sStr;
        }
        public static string pDateBindYMDTime(object oDate)
        {
            string sStr = "";
            if (!string.IsNullOrEmpty(Convert.ToString(oDate)))
            {
                DateTime dTemp = DateTime.Parse(Convert.ToString(oDate));

                string sMonth = dTemp.Month.ToString().PadLeft(2, '0');
                string sDay = dTemp.Day.ToString().PadLeft(2, '0');
                string sHour = dTemp.Hour.ToString().PadLeft(2, '0');
                string sMin = dTemp.Minute.ToString().PadLeft(2, '0');
                string sSec = dTemp.Second.ToString().PadLeft(2, '0');

                sStr = dTemp.Year + sMonth + sDay + sHour + sMin + sSec;
            }
            return sStr;
        }   //20211213 KHOMAS
        public static string pFormatDec(object oParam, int iDec)
        {
            decimal dcParam = pRound(oParam, iDec);
            return string.Format("{0:n" + iDec + "}", dcParam);
        }
        public static decimal pRound(object oParam, object oDec)
        {
            int iDec = pConvInt(oDec);
            decimal dcParam = 0;
            string sParam = "";

            if (!string.IsNullOrEmpty(Convert.ToString(oParam)))
            {
                dcParam = Decimal.Parse((Convert.ToString(oParam)), System.Globalization.NumberStyles.Any);
            }

            dcParam = decimal.Round(dcParam, iDec, MidpointRounding.AwayFromZero);

            sParam = dcParam.ToString();
            sParam = sParam.Contains(".") ? sParam.TrimEnd('0').TrimEnd('.') : sParam;

            return pConvDec(sParam);
        }
        public static string pLastNumber(string lastNum, int startNum, int runNum)
        {
            int iLoop;
            string sLastNumber = "";
            startNum = startNum - 1;

            if (runNum > 0 && startNum > 0 && lastNum.Length <= 13 && lastNum.Length - startNum - runNum > -2)
            {
                for (iLoop = 0; iLoop < runNum - 1; iLoop++)
                {
                    if (isNumeric(lastNum.Substring(startNum + iLoop, runNum - iLoop)))
                    {
                        sLastNumber = lastNum.Substring(0, startNum);
                        sLastNumber += (Convert.ToString(Convert.ToInt32(lastNum.Substring(startNum + iLoop, runNum - iLoop)) + 1)).PadLeft(runNum, '0');
                        sLastNumber += lastNum.Substring(lastNum.Length - startNum - runNum, lastNum.Length - startNum - runNum);
                        return sLastNumber;
                    }
                }
            }
            return sLastNumber;
        }

        //Convert Object To DataType
        public static int pConvInt(object value)
        {
            if (value == null || value == "")
            {
                value = 0;
            }
            return Convert.ToInt32(value);
        }
        public static char pConvChr(object value)
        {
            return Convert.ToChar(value);
        }
        public static string pConvStr(object value)
        {
            return Convert.ToString(value);
        }        
        public static decimal pConvDec(object value)
        {
            return Convert.ToDecimal(value);
        }        
        public static DateTime pConvDate(object value)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                value = null;
            }

            return Convert.ToDateTime(value);
        }
        public static bool pConvBool(object value)
        {
            return Convert.ToBoolean(value);
        }
    }
}