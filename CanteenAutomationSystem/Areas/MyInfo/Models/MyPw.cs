using System;
using System.ComponentModel.DataAnnotations;

namespace CanteenAutomationSystem.Areas.MyInfo.Models
{
    public class MyPw
    {
        [Required(ErrorMessage = "Please enter Password.")]
        public String PASSWORD { get; set; }

        [Required(ErrorMessage = "Please Confirm Password.")]
        public String CPASSWORD { get; set; }
    }
}