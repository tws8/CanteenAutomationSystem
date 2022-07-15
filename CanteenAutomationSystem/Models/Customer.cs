using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Orders = new HashSet<Order>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer User ID required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public String CustID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [Column(TypeName = "VARCHAR")]
        [DataType(DataType.Password)]
        public String CustPW { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer Name required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String CustName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer Membership Availability required")]
        [Column(TypeName = "CHAR")]
        [StringLength(1)]
        public String CustMemberStatus { get; set; }

        [Phone]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        public String CustPhone { get; set; }

        [EmailAddress]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String CustEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer Balance Credit required")]
        public Decimal BalCredit { get; set; }

        [ForeignKey("Customer")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}