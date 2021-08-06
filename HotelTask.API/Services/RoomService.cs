using AutoMapper;
using HotelTask.API.DTOs;
using HotelTask.API.Interfaces;
using HotelTask.Data.DbContexts;
using HotelTask.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelTask.API.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoomService> _logger;
        private IMapper _mapper;
        private IImageService _imageService;
        public RoomService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            IMapper mapper, ILogger<RoomService> logger, IImageService imageService)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _imageService = imageService;
        }
        public async Task<RoomDto> AddRoomAsync(AddRoomDto model, string adminId)
        {
            string upload = default;
            if (model == null || adminId == null) throw new ArgumentNullException(nameof(model));
            var admin = await _userManager.FindByIdAsync(adminId);
            if (!string.IsNullOrEmpty(model.ImageUrl)) upload = await _imageService.UploadCustomerAvatar(model.ImageUrl);
            
           
            var roomToBeCreated = new Room
            {
                CreatedBy = $"{admin.FirstName} {admin.LastName}",
                CreatedDate = DateTime.Now,
                Description = model.Description,
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                RoomNumber = model.RoomNumber,
                RoomPrice = model.RoomPrice,
                ImageUrl = upload,
                IsAvailable = true,
                Availability = "Available"
            };
            try
            {
                await _context.Rooms.AddAsync(roomToBeCreated);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
            var roomToReturn = new RoomDto
            {
                Id = roomToBeCreated.Id,
                Name = roomToBeCreated.Name,
                RoomPrice = roomToBeCreated.RoomPrice,
                RoomNumber = roomToBeCreated.RoomNumber,

            };
            return roomToReturn;
        }

        public async Task<RoomBooking> BookARoomAsync(string roomId, string userId, int numberOfDays)
        {
            if (roomId == null && userId == null) throw new ArgumentNullException();
            RoomBookings bookedRoom = default;
            var user = await _userManager.FindByIdAsync(userId);
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
            if (room.IsAvailable == false)
            {
                var errorToReturn = new RoomBooking
                {
                    ErrorMessage = "Room is already Book Please find a vacant room"
                };
                return errorToReturn;
            }
            else
            {
                room.IsAvailable = false;
                room.Availability = "Not Available";
                 bookedRoom = new RoomBookings
                {
                    Id = Guid.NewGuid().ToString(),
                    NumberOfDays = numberOfDays,
                    Room = room,
                    RoomId = room.Id,
                    User = user,
                    UserId = userId,
                    IsBookingActive = true
                };

                try
                {
                    await _context.RoomBookings.AddAsync(bookedRoom);
                    await _context.SaveChangesAsync();
                    RoomBooking roomBooking = new RoomBooking
                    {
                        BookingId = bookedRoom.Id,
                        DaysBooked = bookedRoom.NumberOfDays,
                        RoomName = bookedRoom.Room.Name,
                        RoomNumber = bookedRoom.Room.RoomNumber,

                    };
                    return roomBooking;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    RoomBooking roomBooking = new RoomBooking
                    {
                        ErrorMessage = ex.Message
                    };
                    return roomBooking;
                }
            }


        }

        public async Task<string> CancelBookingAsync(string bookingId)
        {
            if (bookingId == null) throw new ArgumentNullException(nameof(bookingId));
            var booking = await _context.RoomBookings.FirstOrDefaultAsync(r => r.Id == bookingId);
            if (booking == null) return "No Booking Found";
            booking.IsBookingActive = false;
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == booking.RoomId);
            room.Availability = "Available";
            room.IsAvailable = true;
            await _context.SaveChangesAsync();
            return "Booking Cancelled successfully";
        }

        public async Task<List<RoomDto>> GetAllRoomsAsync()
        {
            List<RoomDto> ListOfRooms = new List<RoomDto>();
            var rooms = await _context.Rooms.ToListAsync();
            foreach (var room in rooms)
            {
                var roomToReturn = new RoomDto
                {
                    Id = room.Id,
                    Description = room.Description,
                    RoomNumber = room.RoomNumber,
                    Name = room.Name,
                    RoomPrice = room.RoomPrice,
                    Availability = room.Availability,
                    ImageUrl = room.ImageUrl
                };
                ListOfRooms.Add(roomToReturn);
            }
            return ListOfRooms;
        }

        public async Task<RoomDto> GetRoomById(string roomId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateRoomAsync(UpdateRoomDto updateRoom)
        {
            string upload = default;
            if (updateRoom == null) throw new ArgumentNullException(nameof(updateRoom));
            var roomToUpdate = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == updateRoom.RoomId);
            if (!string.IsNullOrEmpty(updateRoom.ImageUrl)) upload = await _imageService.UploadCustomerAvatar(updateRoom.ImageUrl);
            if (roomToUpdate == null) return "No Room found that matches this Id";
            else
            {
                roomToUpdate.Name = updateRoom.Name;
                roomToUpdate.Description = updateRoom.Description;
                roomToUpdate.RoomPrice = updateRoom.RoomPrice;
                roomToUpdate.RoomNumber = updateRoom.RoomNumber;
                roomToUpdate.ImageUrl = upload;
                await _context.SaveChangesAsync();
                return "Updated Successfully";
            }
        }


        public async Task<List<GetBookingsDto>> GetAllBookings()
        {
            List<GetBookingsDto> bookingList = new List<GetBookingsDto>();
            var bookings = await _context.RoomBookings.Include(c => c.Room).Include(c => c.User).ToListAsync();
            
            foreach(var booking in bookings)
            {
                var roomWithGuest = new GetBookingsDto
                {
                    BookingId = booking.Id,
                    RoomName = booking.Room.Name,
                    DaysBooked = booking.NumberOfDays,
                    NameOfPersonThatBooked = $"{booking.User.FirstName} {booking.User.LastName}",
                    RoomNumber = booking.Room.RoomNumber,
                    IsBookingActive = booking.IsBookingActive
                };
                bookingList.Add(roomWithGuest);
            }
            return bookingList;
        }

    }
}
