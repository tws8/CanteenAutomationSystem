using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.RecipeManagementSystem.Models
{
    public class PurchaseOrder
    {
        [Required(ErrorMessage = "Please enter ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 ID { get; set; }

        public Int32? VENDOR { get; set; }
        
        [StringLength(50)]
        public String VENDORNAME { get; set; }

        [Required(ErrorMessage = "Please enter Total Price.")]
        public Decimal TOTPRICE { get; set; }

        public DateTime? DTORDDATETIME { get; set; }

        public DateTime? DTVERATETIME { get; set; }

        public DateTime? DTRECDATETIME { get; set; }

        [StringLength(1)]
        public String STATUS { get; set; }
    }
}