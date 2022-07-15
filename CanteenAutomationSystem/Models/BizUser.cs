using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class BizUser
    {
        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Business User ID required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public String BizUserID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [Column(TypeName = "VARCHAR")]
        [DataType(DataType.Password)]
        public String BizUserPW { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Business User Name required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String BizUserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Business User Department required")]
        [Column(TypeName = "CHAR")]
        [StringLength(1)]
        public String BizDeptID { get; set; }

        public virtual BizDept BizDepts { get; set; }
    }
}