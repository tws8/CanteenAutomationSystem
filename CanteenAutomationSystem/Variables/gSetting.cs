using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Variables
{
    public class gSetting
    {
        public String gCurr { get; set; } = "RM";
        public Int32 gDec { get; set; } = 2;
        public Int32 gDiscountRate { get; set; } = 6;

        public Int32 gFmPageSize { get; set; } = 10;
        public Int32 gVwPageSize { get; set; } = 10;
    }
}