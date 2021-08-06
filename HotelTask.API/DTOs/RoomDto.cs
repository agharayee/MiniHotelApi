using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.DTOs
{
    public class RoomDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int RoomNumber { get; set; }
        public decimal RoomPrice { get; set; }
        public string Availability { get; set; }
    }
}
