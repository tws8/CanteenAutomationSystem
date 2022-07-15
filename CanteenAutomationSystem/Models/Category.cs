using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CanteenAutomationSystem.Models
{
    public class Category
    {
        public Category()
        {
            this.Foods = new HashSet<Food>();
        }

        [Key]
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category ID required")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public String CategoryID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public String CategoryDesc { get; set; }

        [ForeignKey("Category")]
        public virtual ICollection<Food> Foods { get; set; }
    }
}