using HotelTask.API.DTOs;
using HotelTask.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelTask.API.Controllers
{
    [ApiController]
    [Route("api/room")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomController> _logger;
        public RoomController(IRoomService roomService, ILogger<RoomController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        [HttpPost("AddRoom")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> AddRoomAsync(AddRoomDto model)
        {
            if (model == null) return BadRequest();
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newRoom = await _roomService.AddRoomAsync(model, adminId);
            _logger.LogInformation($"{newRoom.Name} Room was created Succefully");
            return Created("Room Created Successfully", new
            {
                RoomId = $"{newRoom.Id}",
                RoomName = $"{newRoom.Name}",
                
            });

        }

        [HttpPost("BookRoom")]
        [Authorize(Roles = "Guest")]
        public async Task<ActionResult> BookRoom([FromBody] BookRoomDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (model == null) return BadRequest();
            var bookedRoom = await _roomService.BookARoomAsync(model.Roomid, userId, model.NumberOfDays);
            if (bookedRoom.ErrorMessage != null) return BadRequest(bookedRoom.ErrorMessage);
            _logger.LogError(bookedRoom.ErrorMessage);
            return Ok(bookedRoom);
        }

        [HttpGet("GetAllRooms")]   
        public async Task<IActionResult> GetAllRoomAsync()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            if (rooms == null) return Ok("No Room found");
            return Ok(rooms);
        }

        [HttpPost("CancelBooking")]
        [Authorize(Roles = "Administrator, Guest")]
        public async Task<IActionResult> CancelBookingAsync(string bookingId)
        {
            if (bookingId == null) return BadRequest();
            var result = await  _roomService.CancelBookingAsync(bookingId);
            return Ok(result);
        }

        [HttpPut("UpdateRoom")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateRoomAsync([FromBody]UpdateRoomDto model)
        {
            if (model == null) return BadRequest();
            var updateRoom = await _roomService.UpdateRoomAsync(model);
            return Ok(updateRoom);
        }

        [HttpGet("GetAllBookings")]
        [Authorize(Roles = "Super_Administrator")]
        public async Task<IActionResult> GetAllBookingAsync()
        {
            var booking = await _roomService.GetAllBookings();
            return Ok(booking);
        }

       
    }
}
