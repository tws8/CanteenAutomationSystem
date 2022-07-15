using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Variables
{
    public class NewUser
    {
        [Display(Name = "ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter ID.")]
        [StringLength(15)]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public String ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Name.")]
        [StringLength(50)]
        public String NAME { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Password.")]
        [DataType(DataType.Password)]
        [StringLength(30)]
        public String PASSWORD { get; set; }

        [StringLength(1)]
        //Initial Membership Status is No
        public String STATUS = "N";

        [StringLength(15)]
        public String TEL { get; set; }

        [StringLength(50)]
        public String EMAIL { get; set; }

        //Only applicable to Business User
        [StringLength(1)]
        public String DEPT { get; set; }
    }
}