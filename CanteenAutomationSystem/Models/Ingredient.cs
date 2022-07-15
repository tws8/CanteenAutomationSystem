using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.FoodDets = new HashSet<FoodDet>();
            this.PurchaseOrderDets = new HashSet<PurchaseOrderDet>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingredient ID required")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 IngredientID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingredient Description required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String IngredientDesc { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ingredient Quantity required")]
        public Int32 IngredientQty { get; set; }

        [ForeignKey("IngredientID")]
        public virtual ICollection<FoodDet> FoodDets { get; set; }

        [ForeignKey("IngredientID")]
        public virtual ICollection<PurchaseOrderDet> PurchaseOrderDets { get; set; }
    }
}