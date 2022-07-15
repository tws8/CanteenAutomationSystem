using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.FoodOrderingSystem.Models
{
    public class OrderDet
    {
        [Required(ErrorMessage = "Please enter ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 ID { get; set; }

        [Required(ErrorMessage = "Please enter Item Number.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 ORDERDETID { get; set; }

        [Required(ErrorMessage = "Please enter Food ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 FOODID { get; set; }

        public String FOODDESC { get; set; }

        public Int32 QUANTITY { get; set; }

        public Decimal TOTPRICE { get; set; }

        public String STATUS { get; set; }
    }
}