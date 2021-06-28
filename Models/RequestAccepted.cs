using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.Models
{
    public class RequestAccepted
    {
        [Key]
        public int RejectID { get; set; }

        public int UserID { get; set; }

        public int RequestID { get; set; }

        public int ExecutiveID { get; set; }
    }
}
