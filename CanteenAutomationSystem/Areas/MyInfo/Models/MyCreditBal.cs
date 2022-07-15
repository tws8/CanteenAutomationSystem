using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.MyInfo.Models
{
    public class MyCreditBal
    {
        [StringLength(15)]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public String CUSTOMERID { get; set; }

        public Decimal CREDITBAL { get; set; }
    }
}