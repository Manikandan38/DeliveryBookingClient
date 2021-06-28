using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DeliveryBookingCllient.Models;

namespace DeliveryBookingCllient.Data
{
    public class DeliveryBookingClientContext : DbContext
    {
        public DeliveryBookingClientContext (DbContextOptions<DeliveryBookingClientContext> options)
            : base(options)
        {
        }

        public DeliveryBookingClientContext()
        {

        }
        public DbSet<DeliveryBookingCllient.Models.Admin> Admin { get; set; }
        public DbSet<DeliveryBookingCllient.Models.RequestAccepted> RequestAccepted { get; set; }

        public DbSet<DeliveryBookingCllient.Models.RequestRejected> RequestRejected { get; set; }

        public DbSet<DeliveryBookingCllient.Models.DeliveryStatus> DeliveryStatus { get; set; }



        //public DbSet<DeliveryBookingCllient.Models.ExecutiveResponse> ExecutiveResponse { get; set; }
        //public DbSet<DeliveryBookingCllient.Models.Rejected> RequestRejected { get; set; }
        //public DbSet<DeliveryBookingCllient.Models.ExecutiveResponse> ExecutiveResponse { get; set; }
    }
}
