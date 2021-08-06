using HotelTask.API.Authorizations;
using HotelTask.API.DTOs;
using HotelTask.API.Interfaces;
using HotelTask.Data.DbContexts;
using HotelTask.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelTask.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
                IConfiguration configuration, ApplicationDbContext context, IImageService imageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _imageService = imageService;
        }

        public async Task<LoginSucessfulDto> LoginAsync(LoginDto model)
        {
            JwtSecurityToken token = default;
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSiginKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                token = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   expires: DateTime.Now.AddHours(2),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256Signature)
                   ); ;
            }
            else
            {
                var errorMessage = new LoginSucessfulDto
                {
                    ErrorMessage = "Invalid Crentials"
                };
                return errorMessage;
            }

            var loginDto = new LoginSucessfulDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo.ToString("yyyy-MM-ddThh:mm:ss"),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return loginDto;

        }

        public async Task<RegistrationDto> RegisterAsync(RegisterDto model)
        {
            string upload = default;
            if (!string.IsNullOrEmpty(model.ImageUrl)) upload = await _imageService.UploadCustomerAvatar(model.ImageUrl);
            RegistrationDto returnDto = default;
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageUrl = upload,
                PhoneNumber = model.PhoneNumber
            };
            returnDto = new RegistrationDto
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber

            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Authorization.default_Guestrole.ToString());
                    var roles = await _userManager.GetRolesAsync(user);
                    returnDto.UserRoles = (List<string>)roles;
                    return returnDto;
                }
                else
                {
                    var errors = AddErrors(result);
                    returnDto.ErrorMessage = errors;
                    return returnDto;
                }

            }
            else
            {
                var error = $"Email {user.Email } is already registered.";
                returnDto.ErrorMessage = error;
                return returnDto;
            }
        }

        public async void UpdateUserDetails(RegisterDto customer)
        {
            throw new NotImplementedException();
        }

        

        public async Task<RegistrationDto> AddAdminToRoleAsync(string email)
        {
            if (email == null)
            {
                var error = new RegistrationDto
                {
                    ErrorMessage = "Email cannot be null"
                };
                return error;
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var noUserFound = new RegistrationDto
                {
                    ErrorMessage = "No user found with this Email"
                };
                return noUserFound;
            }
            await _userManager.AddToRoleAsync(user, "Administrator");
            var roles = await _userManager.GetRolesAsync(user);
            var addedSuccessfully = new RegistrationDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserRoles = (List<string>)roles
            };
            return addedSuccessfully;
        }

        public async Task<string> UpdateAccount(UpdateAccountDto updateAccout,string userId)
        {
            string upload = default;
            if (updateAccout == null) throw new ArgumentNullException(nameof(updateAccout));
            var userAccount = await _userManager.FindByIdAsync(userId);
            if (!string.IsNullOrEmpty(updateAccout.ImageUrl)) upload = await _imageService.UploadCustomerAvatar(updateAccout.ImageUrl);
            if (userAccount == null) return "No User Found";
            else
            {
                userAccount.FirstName = updateAccout.FirstName;
                userAccount.LastName = updateAccout.LastName;
                userAccount.ImageUrl = upload;
                userAccount.PhoneNumber = updateAccout.PhoneNumber;
                await _context.SaveChangesAsync();
                return "Updated Successfully";
            }
        }

        public async Task<string> DeleteUserAccount(string userEmail)
        {
            if (userEmail == null) throw new ArgumentNullException(nameof(userEmail));
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) return $"No User Found with this Email {userEmail}";
            var deletedUser = await _userManager.DeleteAsync(user);
            if (deletedUser.Succeeded) return "User Deleted Successfully";
            else return "Something Went wrong";
        }

        /// <summary>
        /// Grabs Registration errors
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string AddErrors(IdentityResult result)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var error in result.Errors)
            {
                sb.Append(error.Description + " Registration Failed. ");
            }
            return sb.ToString();
        }
    }
}
