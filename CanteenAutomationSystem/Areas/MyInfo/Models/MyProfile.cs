using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.MyInfo.Models
{
    public class MyProfile
    {
        [StringLength(15)]
        [RegularExpression(@"^[\w_\!\~\`\@\$\*\(\)\-\.\ \/\#\+]+$", ErrorMessage = "Symbol support !~`@$*()-_. /#+ only")]
        public String ID { get; set; }

        [Required(ErrorMessage = "Please enter Name.")]
        [StringLength(30)]
        public String NAME { get; set; }

        [StringLength(15)]
        public String TEL { get; set; }

        [StringLength(50)]
        public String EMAIL { get; set; }


        //Only applicable to Customer User
        [StringLength(1)]
        public String STATUS { get; set; }

        public Decimal CREDITBAL { get; set; }

        //Only applicable to Business User
        [StringLength(1)]
        public String DEPT { get; set; }
    }
}