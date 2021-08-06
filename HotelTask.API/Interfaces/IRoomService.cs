using HotelTask.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.Interfaces
{
    public interface IRoomService
    {
        Task<RoomDto> AddRoomAsync(AddRoomDto model, string adminId);
        Task<List<RoomDto>> GetAllRoomsAsync();
        Task<RoomDto> GetRoomById(string roomId);
        Task<RoomBooking> BookARoomAsync(string roomId, string userId, int numberOfDays);
        Task<string> CancelBookingAsync(string bookingId);
        Task<string> UpdateRoomAsync(UpdateRoomDto updateRoom);
        Task<List<GetBookingsDto>> GetAllBookings();

    }
}
