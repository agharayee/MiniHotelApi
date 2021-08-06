using HotelTask.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTask.API.Interfaces
{
    public interface IAccountService
    {
        Task<RegistrationDto> RegisterAsync(RegisterDto model);
        Task<LoginSucessfulDto> LoginAsync(LoginDto model);
        void UpdateUserDetails(RegisterDto customer);
        Task<RegistrationDto> AddAdminToRoleAsync(string email);
        Task<string> UpdateAccount(UpdateAccountDto updateAccout, string userId);
        Task<string> DeleteUserAccount(string userEmail);
    }
}
