using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelTask.Data.Entities
{
    public class RoomBookings
    {
        public string Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public virtual Room Room { get; set; }
        public string RoomId { get; set; }
        public int NumberOfDays { get; set; }
        public bool IsBookingActive { get; set; }
    }
}
