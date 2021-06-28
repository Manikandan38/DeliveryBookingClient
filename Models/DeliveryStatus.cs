using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBookingCllient.Models
{
    public class DeliveryStatus
    {
        [Key]
        public int DeliveryID { get; set; }
        public int UserID { get; set; }
        public int RequestID { get; set; }
        public int ExecutiveID { get; set; }
        public bool Received { get; set; }
        public bool Shipped { get; set; }
        public bool Delivered { get; set; }

    }
}
