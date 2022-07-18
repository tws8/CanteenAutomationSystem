using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.RecipeManagementSystem.Models
{
    public class PurchaseOrderDet
    {
        [Required(ErrorMessage = "Please enter ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 ID { get; set; }

        [Required(ErrorMessage = "Please enter Item Number.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 PURCHASEORDERDETID { get; set; }

        [Required(ErrorMessage = "Please enter Ingredient ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 INGREDIENTID { get; set; }

        [StringLength(50)]
        public String INGREDIENTDESC { get; set; }

        public Decimal UNITPRICE { get; set; }

        public Int32 QUANTITY { get; set; }

        public Decimal TOTPRICE { get; set; }

        [StringLength(1)]
        public String STATUS { get; set; }
    }
}