using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rent_products_api.DataLayer.DTOs.Email;
using rent_products_api.DataLayer.DTOs.User;
using rent_products_api.DataLayer.Utils;
using rent_products_api.DBContexts;
using rent_products_api.Models;
using rent_products_api.ServiceLayer;
using rent_products_api.ServiceLayer.UsersService;
using rent_products_api.ServiceLayer.Utils;

namespace rent_products_api.Controllers
{
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/Users/Login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequestDTO model)
        {
            var authenticatedUser = await _userService.Authenticate(model, ipAddress());
            if (authenticatedUser.Success)
            {
                setTokenCookie(authenticatedUser.Response.RefreshToken);
                switch (authenticatedUser.Response.UserType)
                {
                    case UserType.AdminUser:
                        return Ok(new { Response = (AdminUserDTO)authenticatedUser.Response, Message = authenticatedUser.Message, Success = authenticatedUser.Success });
                    case UserType.SimpleUser:
                        return Ok(new { Response = (UserDTO)authenticatedUser.Response, Message = authenticatedUser.Message, Success = authenticatedUser.Success });
                    default:
                        return Ok(new { Response = (string)null, Message = authenticatedUser.Message, Success = authenticatedUser.Success });
                }
            }
            else
            {
                return BadRequest(authenticatedUser);
            }
        }

        [HttpPost("/Users/RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _userService.RefreshToken(refreshToken, ipAddress());
            if (result.Success)
            {
                setTokenCookie(result.Response.RefreshToken);
                switch (result.Response.UserType)
                {
                    case UserType.AdminUser:
                        return Ok(new { Response = (AdminUserDTO)result.Response, Message = result.Message, Success = result.Success });
                    case UserType.SimpleUser:
                        return Ok(new { Response = (UserDTO)result.Response, Message = result.Message, Success = result.Success });
                    default:
                        return Ok(new { Response = (string)null, Message = result.Message, Success = result.Success });
                }
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("/Users/ResendVerificationEmail")]
        public async Task<IActionResult> ResendVerificationEmail([FromQuery] string email)
        {

            var result = await _userService.ResendVerificationEmail(email);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/RevokeToken")]
        public async Task<IActionResult> RevokeToken(string tokenToReset)
        {
            // accept token from request body or cookie
            var token = tokenToReset ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and admins can revoke any tokens
            if (!Account.OwnsToken(token) && Account.UserType != UserType.AdminUser)
                return Unauthorized(new { message = "Unauthorized" });

            var result = await _userService.RevokeToken(token, ipAddress());
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO model)
        {
            var result = await _userService.RegisterUser(model, Request.Headers["origin"]);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminUserDTO model)
        {
            var result = await _userService.RegisterAdminUser(model, Request.Headers["origin"]);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] AccountVerifyDTO accountVerify)
        {
            var result = await _userService.VerifyEmail(accountVerify);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO data)
        {
            var result = await _userService.ForgotPassword(data.Email, Request.Headers["origin"]);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/ValidateResetToken")]
        public async Task<IActionResult> ValidateResetToken([FromQuery] string token)
        {
            var result = await _userService.ValidateResetToken(token);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var result = await _userService.ResetPassword(model);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize(UserType.AdminUser)]
        [HttpGet("/Users/GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAll();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize]
        [HttpGet("/Users/GetUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // users can get their own account and admins can get any account
            //if (id != Account.UserId && Account.UserType != UserType.AdminUser)
            //    return Unauthorized(new { message = "Unauthorized" });

            var result = await _userService.GetById(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize(UserType.AdminUser)]
        [HttpPost("/Users/CreateUser")]
        public async Task<IActionResult> Create(ManualUserCreationDTO model)
        {
            var result = await _userService.Create(model);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize]
        [HttpPost("/Users/UpdateUser")]
        public async Task<IActionResult> Update(UpdateUserDTO model)
        {
            Guid userLoggedInId= new Guid();
            if (Account != null)
            {
                userLoggedInId = Account.UserId; 
            }
            // users can update their own account and admins can update any account
            if (Account!=null && model.UserId != Account.UserId && Account.UserType != UserType.AdminUser)
                return Unauthorized(new { message = "Unauthorized" });

            var result = await _userService.Update(model,userLoggedInId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize(UserType.AdminUser)]
        [HttpPost("/Users/DeleteUser")]
        public async Task<IActionResult> Delete([FromForm]Guid id)
        {
            // users can delete their own account and admins can delete any account
            if (Account!=null && id != Account.UserId && Account.UserType != UserType.AdminUser)
                return Unauthorized(new { message = "Unauthorized" });

            var result = await _userService.Delete(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost("/Users/SendContactEmail")]
        public async Task<IActionResult> SendContactEmail([FromBody] ContactEmailDTO dto)
        {

            var result = await _userService.SendContactEmail(dto);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [Authorize(UserType.AdminUser)]
        [HttpGet("/Users/GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsers();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }


        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = GenericFunctions.GetCurrentDateTime().AddDays(7),
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
