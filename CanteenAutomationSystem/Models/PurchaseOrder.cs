using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class PurchaseOrder
    {
        public PurchaseOrder()
        {
            this.PurchaseOrderDets = new HashSet<PurchaseOrderDet>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Purchase Order required")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 PurchaseOrderID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vendor ID required")]
        public Int32 Vendor { get; set; }

        public Decimal TotalPrice { get; set; }

        [StringLength(1)]
        public String Status { get; set; }

        public virtual Vendor Vendors { get; set; }

        [ForeignKey("PurchaseOrderID")]
        public virtual ICollection<PurchaseOrderDet> PurchaseOrderDets { get; set; }
    }
}