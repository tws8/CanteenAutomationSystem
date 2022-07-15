using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.FoodMenuSystem.Models
{
    public class FoodMenu
    {
        [Required(ErrorMessage = "Please enter ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 ID { get; set; }

        [Required(ErrorMessage = "Please enter Description.")]
        [StringLength(20)]
        public String DESC { get; set; }

        [Required(ErrorMessage = "Please enter Price.")]
        public Decimal PRICE { get; set; }

        [StringLength(50)]
        public String REM { get; set; }

        [Required(ErrorMessage = "Please enter Category.")]
        [StringLength(10)]
        public String CATEGORY { get; set; }

        public String URL { get; set; }

        [Required(ErrorMessage = "Please enter Status.")]
        [StringLength(1)]
        public String STATUS { get; set; }
    }
}