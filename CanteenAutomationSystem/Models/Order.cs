using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderDets = new HashSet<OrderDet>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Food Order required")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 OrderID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer ID required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        public String Customer { get; set; }

        public Int32? PayID { get; set; }

        [Column(TypeName = "CHAR")]
        [StringLength(1)]
        public String Delivery { get; set; }

        public Decimal TotalPrice { get; set; }

        public Decimal Discount { get; set; }

        public Decimal ActPrice { get; set; }

        public DateTime? OrderedDateTime { get; set; }

        public DateTime? EstPreparedDateTime { get; set; }

        public DateTime? ActPreparedDateTime { get; set; }

        public DateTime? ActDeliveryDateTime { get; set; }

        public Int32? CustRating { get; set; }

        [StringLength(1)]
        public String Status { get; set; }

        public virtual Customer Customers { get; set; }

        public virtual Pay Pays { get; set; }

        [ForeignKey("OrderID")]
        public virtual ICollection<OrderDet> OrderDets { get; set; }
    }
}