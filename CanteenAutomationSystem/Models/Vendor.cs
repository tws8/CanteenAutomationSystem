using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class Vendor
    {
        public Vendor()
        {
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Food ID required")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 VendorID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vendor Name required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String VendorName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String VendorContact { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String VendorEmail { get; set; }

        [ForeignKey("Vendor")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}