using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.MyInfo.Models
{
    public class MyPw
    {
        [Required(ErrorMessage = "Please enter Password.")]
        [StringLength(30)]
        public String PASSWORD { get; set; }

        [Required(ErrorMessage = "Please Confirm Password.")]
        [StringLength(30)]
        public String CPASSWORD { get; set; }
    }
}