using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.DTOs
{
    public class UpdateRoomDto : AddRoomDto
    {
        public string RoomId { get; set; }
    }

}
