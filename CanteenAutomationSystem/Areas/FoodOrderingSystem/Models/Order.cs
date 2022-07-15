using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.FoodOrderingSystem.Models
{
    public class Order
    {
        [Required(ErrorMessage = "Please enter ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 ID { get; set; }

        [StringLength(1)]
        public String PAYMENT { get; set; }

        [StringLength(1)]
        public String DELIVERY { get; set; }

        [Required(ErrorMessage = "Please enter Total Price.")]
        public Decimal TOTPRICE { get; set; }

        [Required(ErrorMessage = "Please enter Discount.")]
        public Decimal DISCOUNT { get; set; }

        [Required(ErrorMessage = "Please enter Actual Price.")]
        public Decimal ACTPRICE { get; set; }

        public DateTime? DTORDDATETIME { get; set; }

        public DateTime? DTESTDATETIME { get; set; }

        public DateTime? DTACTDATETIME { get; set; }

        public DateTime? DTDELDATETIME { get; set; }

        [StringLength(1)]
        public String STATUS { get; set; }
    }
}