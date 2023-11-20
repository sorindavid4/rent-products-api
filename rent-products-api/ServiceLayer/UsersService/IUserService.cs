using Microsoft.AspNetCore.Mvc;
using rent_products_api.DataLayer.DTOs.Email;
using rent_products_api.DataLayer.DTOs.User;
using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.UsersService
{
    public interface IUserService
    {
        Task<ServiceResponse<BaseUserDTO>> Authenticate(AuthenticateRequestDTO model, string ipAddress);
        Task<ServiceResponse<BaseUserDTO>> RefreshToken(string token, string ipAddress);
        Task<ServiceResponse<Object>> RevokeToken(string token, string ipAddress);
        Task<ServiceResponse<Object>> RegisterUser(RegisterUserDTO model, string origin);
        
        //remove in production
        Task<ServiceResponse<Object>> RegisterAdminUser(RegisterAdminUserDTO model, string origin);
        
        Task<ServiceResponse<Object>> VerifyEmail(AccountVerifyDTO accountVerifyDTO);
        Task<ServiceResponse<Object>> ForgotPassword(string email, string origin);
        Task<ServiceResponse<Object>> ValidateResetToken(string token);
        Task<ServiceResponse<Object>> ResetPassword(ResetPasswordDTO model);
        Task<ServiceResponse<IEnumerable<BaseUserDTO>>> GetAll();
        Task<ServiceResponse<Object>> GetById(Guid id);
        Task<ServiceResponse<BaseUserDTO>> Create(ManualUserCreationDTO model);
        Task<ServiceResponse<BaseUserDTO>> Update( UpdateUserDTO model, Guid userLoggedInId);
        Task<ServiceResponse<Object>> Delete(Guid id);
        Task<ServiceResponse<Object>> SendContactEmail(ContactEmailDTO dto);
        Task<ServiceResponse<Object>> ResendVerificationEmail(string email);
        Task<ServiceResponse<IEnumerable<SimpleUserDTO>>> GetUsers();
    }
}
