using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.Models
{
    public class ExecutiveResponse
    {
        [Key]
        public int ResponseID { get; set; }

        [Display(Name ="Click to Accept")]
        public bool status { get; set; }
        [Required]
        public double Price { get; set; }
        public int RequestID { get; set; }
        
    }
}
