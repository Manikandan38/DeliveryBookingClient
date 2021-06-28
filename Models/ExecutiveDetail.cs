using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.Models
{
    public class ExecutiveDetail
    {
        public int ExecutiveID { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Name { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password atleast have 1 Uppercase, 1 Lowercase, 1 Number and 1 Special character")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [MaxLength(length: 10, ErrorMessage = "*Only 10 numbers allowed")]
        [MinLength(length: 10, ErrorMessage = "*Must have 10 numbers")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(length: 30, ErrorMessage = "*Only 30 characters allowed")]
        public string Address { get; set; }
        public bool IsVerified { get; set; }

        [Display(Name = "City")]
        public int CityID { get; set; }

    }
}
