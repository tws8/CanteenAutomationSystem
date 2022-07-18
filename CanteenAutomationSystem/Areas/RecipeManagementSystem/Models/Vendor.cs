using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.RecipeManagementSystem.Models
{
    public class Vendor
    {
        [Required(ErrorMessage = "Please enter ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 ID { get; set; }

        [StringLength(50)]
        public String NAME { get; set; }

        [StringLength(50)]
        public String PHONE { get; set; }

        [StringLength(50)]
        public String EMAIL { get; set; }
    }
}