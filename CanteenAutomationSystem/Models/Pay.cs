using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class Pay
    {
        public Pay()
        {
            this.Orders = new HashSet<Order>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment ID required")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 PayID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer ID required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        public String Customer { get; set; }

        public Decimal OrderPrice { get; set; }

        [ForeignKey("PayID")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual Customer Customers { get; set; }
    }
}