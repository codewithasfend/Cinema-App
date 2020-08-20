using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class ReservationDetail
    {
        public int Id { get; set; }
        public DateTime ReservationTime { get; set; }
        public string CustomerName { get; set; }
        public string MovieName { get; set; }
        public string Email { get; set; }
        public int Qty { get; set; }
        public int Price { get; set; }
        public string Phone { get; set; }
        public string PlayingDate { get; set; }
        public string PlayingTime { get; set; }
    }
}
