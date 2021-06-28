using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.Models
{
    public class City
    {
        [Display(Name = "City ID")]
        public int CityID { get; set; }

        [Required]
        [MinLength(length: 3, ErrorMessage = "*Atleast have 3 characters.")]
        [MaxLength(length: 20, ErrorMessage = "*Only 20 characters allowed.")]
        [Display(Name = "City Name")]
        public string CityName { get; set; }
    }
}
