using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class BizDept : IValidatableObject
    {
        public BizDept()
        {
            this.BizUsers = new HashSet<BizUser>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Dept ID required")]
        [Column(TypeName = "CHAR")]
        [StringLength(1)]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public String BizDeptID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String BizDeptDesc { get; set; }

        [ForeignKey("BizDeptID")]
        public virtual ICollection<BizUser> BizUsers { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //BizDeptID can only be A (Administrator), M (Management), K (Kitchen Staff), F (Financial Staff), R (Receptionist Staff), B (Buyer Staff)
            if (string.IsNullOrEmpty(BizDeptID) || !(BizDeptID.Equals("A") || BizDeptID.Equals("M") || BizDeptID.Equals("K") || BizDeptID.Equals("F") || BizDeptID.Equals("R") || BizDeptID.Equals("B")))
            {
                yield return new ValidationResult("Invalid Business Department ID");
            }
        }
    }
}