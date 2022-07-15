using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.FoodMenuSystem.Models
{
    public class MenuCategory
    {
        [Required(ErrorMessage = "Please enter ID.")]
        [StringLength(10)]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public string ID { get; set; }

        [StringLength(50)]
        public String DESC { get; set; }
    }
}