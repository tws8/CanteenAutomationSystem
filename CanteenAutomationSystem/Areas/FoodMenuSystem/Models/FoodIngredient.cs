using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.FoodMenuSystem.Models
{
    public class FoodIngredient
    {
        [Required(ErrorMessage = "Please enter ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 ID { get; set; }

        [Required(ErrorMessage = "Please enter Item Number.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 FOODDETID { get; set; }

        [Required(ErrorMessage = "Please enter Ingredient ID.")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 INGREDIENTID { get; set; }

        public String INGREDIENTDESC { get; set; }

        [Required(ErrorMessage = "Please enter Quantity.")]
        public Int32 INGREDIENTQTY { get; set; }
    }
}