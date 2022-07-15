using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class OrderDet
    {
        [Key, Column(Order = 1)]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Food Order ID Food No required")]
        public Int32 OrderDetID { get; set; }

        [Key, Column(Order = 0)]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Food Order required")]
        public Int32 OrderID { get; set; }

        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Food ID required")]
        public Int32 FoodID { get; set; }

        public Decimal UnitPrice { get; set; }

        public Int32 Quantity { get; set; }

        public Decimal TotalPrice { get; set; }

        public String Status { get; set; }

        public virtual Order Orders { get; set; }

        public virtual Food Foods { get; set; }
    }
}