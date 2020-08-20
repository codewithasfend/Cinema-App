using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Models
{
    public class MovieDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public string PlayingDate { get; set; }
        public string PlayingTime { get; set; }
        public int TicketPrice { get; set; }
        public double Rating { get; set; }
        public string Genre { get; set; }
        public string TrailorUrl { get; set; }
        public string ImageUrl { get; set; }
        public object Image { get; set; }
        public object Reservations { get; set; }
        public string FullImageUrl => AppSettings.ApiUrl + ImageUrl;
    }
}
