using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class Reservation
    {
        public int Qty { get; set; }
        public int Price { get; set; }
        public string Phone { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
