using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.DTOs
{
    public class GetBookingsDto
    {
        public string BookingId { get; set; }
        public string RoomName { get; set; }
        public int RoomNumber { get; set; }
        public int DaysBooked { get; set; }
        public string NameOfPersonThatBooked { get; set; }
        public bool IsBookingActive { get; set; }
        public string ErrorMessage { get; set; }

    }
}
