using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationTime { get; set; }
        public string CustomerName { get; set; }
        public string MovieName { get; set; }
    }
}
