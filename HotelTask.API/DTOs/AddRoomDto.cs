using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.DTOs
{
    public class AddRoomDto
    {
        [Required(ErrorMessage ="Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }
       
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "RoomNumber is Required")]
        public int RoomNumber { get; set; }
        [Required(ErrorMessage = "RoomPrice is Required")]
        public decimal RoomPrice { get; set; }
    }
}
