using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class Food
    {
        public Food()
        {
            this.OrderDets = new HashSet<OrderDet>();
            this.FoodDets = new HashSet<FoodDet>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Food ID required")]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public Int32 FoodID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Food Description required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String FoodDesc { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Price required")]
        public Decimal FoodPrice { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String FoodRem { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Category required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public String Category { get; set; }

        [Column(TypeName = "VARCHAR")]
        public String Url { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Status required")]
        [Column(TypeName = "CHAR")]
        [StringLength(1)]
        public String Status { get; set; }

        public virtual Category Categorys { get; set; }

        [ForeignKey("FoodID")]
        public virtual ICollection<OrderDet> OrderDets { get; set; }

        [ForeignKey("FoodID")]
        public virtual ICollection<FoodDet> FoodDets { get; set; }
    }
}