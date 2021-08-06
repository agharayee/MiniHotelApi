using HotelTask.API.DTOs;
using HotelTask.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelTask.API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync([FromBody]LoginDto model)
        {
            if (model == null) return BadRequest();
            var result = await _accountService.LoginAsync(model);
            if (result.ErrorMessage != null)  return BadRequest("Invalid Credentials");
            return Ok(result);

            
        }
        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            if (model == null) return BadRequest();
            else
            {
                var result = await _accountService.RegisterAsync(model);
                if (result.ErrorMessage != null)  return BadRequest(result.ErrorMessage);
                return Ok(result);
            }
        }
        [HttpPost("AddToRole")]
        public async Task<ActionResult> AddToAdminRoleAsync([FromQuery] string email)
        {
            var result = await _accountService.AddAdminToRoleAsync(email);
            if (result.ErrorMessage != null) return BadRequest(result.ErrorMessage);
            return Ok(result);
        }

        [HttpPost("UpdateUserDetails")]
        public async Task<ActionResult> UpdateDetialsAsync(UpdateAccountDto update)
        {
            if (update == null) return BadRequest();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedUser = await _accountService.UpdateAccount(update, userId);
            return Ok(updatedUser);
        }

        [HttpPost("DeleteAccount")]
        [Authorize(Roles = "Super_Administrator")]
        public async Task<IActionResult> DeleteAccountAsync(string userEmail)
        {
            if (userEmail == null) return BadRequest();
            var result = await _accountService.DeleteUserAccount(userEmail);
            return Ok(result);
        }
    }
}
