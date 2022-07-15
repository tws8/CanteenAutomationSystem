using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class PurchaseOrderDet
    {
        [Key, Column(Order = 1)]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Purchase Order ID Purchase No required")]
        public Int32 PurchaseOrderDetID { get; set; }

        [Key, Column(Order = 0)]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Purchase Order required")]
        public Int32 PurchaseOrderID { get; set; }

        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingredient ID required")]
        public Int32 IngredientID { get; set; }

        public Decimal UnitPrice { get; set; }

        public Int32 Quantity { get; set; }

        public Decimal IngredientPrice { get; set; }

        public String Status { get; set; }

        public virtual PurchaseOrder PurchaseOrders { get; set; }

        public virtual Ingredient Ingredients { get; set; }
    }
}