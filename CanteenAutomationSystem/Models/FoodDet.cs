using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class FoodDet
    {
        [Key, Column(Order = 1)]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Food ID Food No required")]
        public Int32 FoodDetID { get; set; }

        [Key, Column(Order = 0)]
        [Display(Name = "FoodID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Food ID required")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 FoodID { get; set; }

        [Display(Name = "IngredientID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingredient ID required")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 IngredientID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingredient Quantity required")]
        public Int32 IngredientQty { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Status required")]
        [Column(TypeName = "CHAR")]
        [StringLength(1)]
        public String Status { get; set; }

        public virtual Food Foods { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}