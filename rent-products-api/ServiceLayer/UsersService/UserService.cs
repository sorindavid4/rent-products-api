using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using rent_products_api.DataLayer.DTOs.Email;
using rent_products_api.DataLayer.DTOs.User;
using rent_products_api.DataLayer.Utils;
using rent_products_api.DBContexts;
using rent_products_api.Models;
using rent_products_api.Models.User;
using rent_products_api.ServiceLayer.EmailService;
using rent_products_api.ServiceLayer.UsersService;
using rent_products_api.ServiceLayer.Utils;
using BC = BCrypt.Net.BCrypt;

namespace rent_products_api.ServiceLayer
{
    public class UserService : IUserService
    {
        private readonly MainDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public UserService(MainDbContext context, 
            IMapper mapper, 
            IOptions<AppSettings> appSettings, 
            IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public async Task<ServiceResponse<IEnumerable<BaseUserDTO>>> GetAll()
        {
            try
            {
                var dbUsers = await _context.Users.ToListAsync();
                var response = _mapper.Map<List<BaseUserDTO>>(dbUsers);
                return new ServiceResponse<IEnumerable<BaseUserDTO>> { Response = response, Success = true };
            }
            catch (Exception e)
            {
                return new ServiceResponse<IEnumerable<BaseUserDTO>> { Response = null, Success = false, Message = Messages.Message_UsersLoadError };
            }
            
        }

        public async Task<ServiceResponse<BaseUserDTO>> Authenticate(AuthenticateRequestDTO model, string ipAddress)
        {
            try
            {
                var success = true;
                var account = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email);
                var message = Messages.Message_LoggedInSuccessfully;

                if (account == null  || !BC.Verify(model.Password, account.Password) || account.IsDeleted)
                    throw new AppException("Email or password is incorrect");

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = generateJwtToken(account);
                var refreshToken = generateRefreshToken(ipAddress);
                account.RefreshTokens.Add(refreshToken);

                // remove old refresh tokens from account
                removeOldRefreshTokens(account);

                // save changes to db
                _context.Update(account);
                _context.SaveChanges();

                var response = new BaseUserDTO();
                switch (account.UserType)
                {
                    case UserType.AdminUser:
                        response = _mapper.Map<AdminUserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = refreshToken.Token;
                        break;
                    case UserType.SimpleUser:
                        response = _mapper.Map<UserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = refreshToken.Token;
                        break;
                    default:
                        success = false;
                        message = Messages.Message_LoggedInError;
                        break;
                }

                return new ServiceResponse<BaseUserDTO> { Response = response, Success = success, Message = message };
            }
            catch (Exception e)
            {
                return new ServiceResponse<BaseUserDTO> { Response = null, Success = false, Message = Messages.Message_LoggedInError };
            }
        }

        public async Task<ServiceResponse<BaseUserDTO>> RefreshToken(string token, string ipAddress)
        {
            try
            {
                var (refreshToken, account) = await getRefreshToken(token);
                var success = false;
                var message = Messages.Message_RefreshedTokenSuccess;

                // replace old refresh token with a new one and save
                var newRefreshToken = generateRefreshToken(ipAddress);
                refreshToken.Revoked = GenericFunctions.GetCurrentDateTime();
                refreshToken.RevokedByIp = ipAddress;
                refreshToken.ReplacedByToken = newRefreshToken.Token;
                account.RefreshTokens.Add(newRefreshToken);

                removeOldRefreshTokens(account);

                _context.Update(account);
                _context.SaveChanges();

                // generate new jwt
                var jwtToken = generateJwtToken(account);

                success = true;

                var response = new BaseUserDTO();
                switch (account.UserType)
                {
                    case UserType.AdminUser:
                        response = _mapper.Map<AdminUserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = newRefreshToken.Token;
                        break;
                    case UserType.SimpleUser:
                        response = _mapper.Map<UserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = newRefreshToken.Token;
                        break;
                    default:
                        success = false;
                        message = Messages.Message_RefreshedTokenError;
                        break;
                }

                return new ServiceResponse<BaseUserDTO> { Response = response, Success = success, Message = message };
            }
            catch (Exception e)
            {
                return new ServiceResponse<BaseUserDTO> { Response = null, Success = false, Message = Messages.Message_RefreshedTokenError };
            }
        }

        public async Task<ServiceResponse<Object>> RevokeToken(string token, string ipAddress)
        {
            try
            {
                var (refreshToken, account) = await getRefreshToken(token);
                var message = Messages.Message_RevokeTokenSuccess;

                // revoke token and save
                refreshToken.Revoked = GenericFunctions.GetCurrentDateTime();
                refreshToken.RevokedByIp = ipAddress;
                _context.Update(account);
                _context.SaveChanges();

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = message };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_RevokeTokenError };
            }
        }

        public async Task<ServiceResponse<Object>> RegisterUser(RegisterUserDTO model, string origin)
        {
            try
            {
                var message = Messages.Message_UserRegisteredSuccess;

                // validate
                if (await _context.Users.AnyAsync(x => x.Email == model.Email))
                {
                    // send already registered error in email to prevent account enumeration
                    return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_EmailAlreadyUsed };
                }
                // map model to new account object
                var account = _mapper.Map<User>(model);

                account.UserType = UserType.SimpleUser;
                account.CreatedAt = GenericFunctions.GetCurrentDateTime();
                account.VerificationToken = randomTokenString();
                account.Password = BC.HashPassword(model.Password);
                // save account
                _context.Users.Add(account);
                _context.SaveChanges();

                // send email
                _emailService.SendVerificationEmail(account, origin);
                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = message };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Object> { Response = e.StackTrace.ToString(), Success = false, Message = Messages.Message_UserRegisterError };
            }
        }

        public async Task<ServiceResponse<Object>> RegisterAdminUser(RegisterAdminUserDTO model, string origin)
        {
            try
            {
                var message = Messages.Message_UserRegisteredSuccess;

                // validate
                if (await _context.Users.AnyAsync(x => x.Email == model.Email))
                {
                    // send already registered error in email to prevent account enumeration
                    return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_EmailAlreadyUsed };
                }

                // map model to new account object
                var account = _mapper.Map<AdminUser>(model);

                account.UserType = UserType.AdminUser;
                account.CreatedAt = GenericFunctions.GetCurrentDateTime();
                account.VerificationToken = randomTokenString();
                account.Password = BC.HashPassword(model.Password);

                // save account
                _context.Users.Add(account);
                _context.SaveChanges();

                // send email
                _emailService.SendVerificationEmail(account, origin);

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = message };
            }
            catch (Exception e)
            {
     
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_UserRegisterError };
            }
        }


        public async Task<ServiceResponse<Object>> VerifyEmail(AccountVerifyDTO accountVerifyDTO)
        {
            try
            {
                var account = await _context.Users.SingleOrDefaultAsync(x => x.VerificationToken == accountVerifyDTO.Token);

                if (account == null) throw new AppException("Verification failed");

                account.Verified = TimeZoneInfo.ConvertTime(GenericFunctions.GetCurrentDateTime(), TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
                account.VerificationToken = null;

                var success = await _context.SaveChangesAsync();
                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_EmailVerified };
                
            }
            catch (Exception e)
            {
             
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_EmailVerifyError };
            }
        }
        public async Task<ServiceResponse<object>> ResendVerificationEmail(string email)
        {
            try
            {

                var account = _context.Users.FirstOrDefault(x => x.Email == email);
                if (account == null)
                {
                    return new ServiceResponse<object> { Success = false, Message = Messages.Message_SendVerificationEmailError };
                }
                account.VerificationToken = randomTokenString();

                _context.Users.Update(account);
                _context.SaveChanges();

                _emailService.ResendVerificationEmail(email, account.VerificationToken);

                return new ServiceResponse<object> { Success = true, Message = Messages.Message_SendVerificationEmailSuccess };
            }
            catch (Exception e)
            {
                return new ServiceResponse<object> { Success = false, Message = Messages.Message_SendVerificationEmailError };
            }
        }
        public async Task<ServiceResponse<Object>> ForgotPassword(string email, string origin)
        {
            try
            {
                var account = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

                // always return ok response to prevent email enumeration
                if (account == null) return new ServiceResponse<Object> { Response = (string)null, Success = false }; ;

                // create reset token that expires after 1 day
                account.ResetToken = randomTokenString();
                account.ResetTokenExpires = GenericFunctions.GetCurrentDateTime().AddDays(1);

                _context.Users.Update(account);
                _context.SaveChanges();

                // send email
                _emailService.SendPasswordResetEmail(account, origin);

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_ForgottenPasswordEmailSent };
            }
            catch (Exception e)
            {
               
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_ForgottenPasswordEmailNotSent };
            }
        }

        public async Task<ServiceResponse<Object>> ValidateResetToken(string token)
        {
            try
            {
                var account = await _context.Users.SingleOrDefaultAsync(x =>
                        x.ResetToken == token &&
                        x.ResetTokenExpires > GenericFunctions.GetCurrentDateTime());

                if (account == null)
                    throw new AppException("Invalid token");

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_ValidateResetTokenSuccess };
            }
            catch (Exception e)
            {
       
                return new ServiceResponse<object> { Response = null, Success = false, Message = Messages.Message_ValidateResetTokenError };
            }
        }

        public async Task<ServiceResponse<Object>> ResetPassword(ResetPasswordDTO model)
        {
            try
            {
                var account = await _context.Users.SingleOrDefaultAsync(x =>
                        x.ResetToken == model.Token &&
                        x.ResetTokenExpires > GenericFunctions.GetCurrentDateTime());

                if (account == null)
                    throw new AppException("Invalid token");

                // update password and remove reset token
                    account.Password = BC.HashPassword(model.Password);
                account.PasswordReset = GenericFunctions.GetCurrentDateTime();
                account.ResetToken = null;
                account.ResetTokenExpires = null;

                _context.Users.Update(account);
                _context.SaveChanges();

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_ResetPasswordSuccess };
            }
            catch (Exception e)
            {
       
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_ResetPasswordError };
            }
        }

        public async Task<ServiceResponse<Object>> GetById(Guid id)
        {
            try
            {
                var account = await getAccount(id);

                if (account == null)
                {
                    return new ServiceResponse<object> { Response = null, Success = false, Message = Messages.Message_UserDataLoadError };
                }

                var response = new BaseUserDTO();

                switch (account.UserType)
                {
                    case UserType.AdminUser:
                        response = _mapper.Map<AdminUserDTO>(account);
                        break;
                    case UserType.SimpleUser:
                        response = _mapper.Map<UserDTO>(account);
                        response.Created = GenericFunctions.ParseDateTime(account.CreatedAt);
                        response.Updated = GenericFunctions.ParseNullableDateTime(account.Updated);
                        break;
                    default:
                        return new ServiceResponse<object> { Response = null, Success = false, Message = Messages.Message_UserDataLoadError };
                        break;
                }

                return new ServiceResponse<object> { Response = response, Success = true };
            }
            catch (Exception e)
            {
     
                return new ServiceResponse<object> { Response = null, Success = false, Message = Messages.Message_UserDataLoadError };
            }
        }

        public async Task<ServiceResponse<BaseUserDTO>> Create(ManualUserCreationDTO model)
        {
            try
            {
                // validate
                if (await _context.Users.AnyAsync(x => x.Email == model.Email))
                    throw new AppException($"Email '{model.Email}' is already registered");

                // map model to new account object
                var account = _mapper.Map<BaseUser>(model);
                account.CreatedAt = GenericFunctions.GetCurrentDateTime();
                account.Verified = GenericFunctions.GetCurrentDateTime();

                // hash password
                account.Password = BC.HashPassword(model.Password);

                // save account
                _context.Users.Add(account);
                _context.SaveChanges();

                var result = _mapper.Map<BaseUserDTO>(account);

                return new ServiceResponse<BaseUserDTO> { Response = result, Success = true, Message = Messages.Message_UserRegisteredSuccess };
            }
            catch (Exception e)
            {
          
                return new ServiceResponse<BaseUserDTO> { Response = null, Success = false, Message = Messages.Message_UserRegisterError };
            }

        }

        public async Task<ServiceResponse<BaseUserDTO>> Update(UpdateUserDTO model, Guid userLoggedInId)
        {
            try
            {
                var account = await getAccount(model.UserId);

                // validate
                if (account.Email != model.Email && _context.Users.Any(x => x.Email == model.Email))
                    throw new AppException($"Email '{model.Email}' is already taken");

                switch (account.UserType)
                {
                    case UserType.SimpleUser:
                        var cAccount = (User)account;
                        break;
                }

                // copy model to account and save
                _mapper.Map(model, account);
                
                account.Updated = GenericFunctions.GetCurrentDateTime();
                _context.Users.Update(account);
                _context.SaveChanges();

                var result = _mapper.Map<BaseUserDTO>(account);

                return new ServiceResponse<BaseUserDTO> { Response = result, Success = true, Message = Messages.Message_UserUpdateSuccess };
            }
            catch (Exception e)
            {
               
                return new ServiceResponse<BaseUserDTO> { Response = null, Success = false, Message = Messages.Message_UserUpdateError };
            }
        }

        public async Task<ServiceResponse<Object>> Delete(Guid id)
        {
            try
            {
                var account = await getAccount(id);
                account.IsDeleted = true;
                _context.SaveChanges();

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_UserDeleteSuccess };
            }
            catch (Exception e)
            {
               
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_UserDeleteError };
            }
        }
        private async Task<BaseUser> getAccount(Guid id)
        {
            var account = await _context.Users.FindAsync(id);
            if (account == null) throw new KeyNotFoundException("Account not found");
            return account;
        }


        private async Task<(RefreshToken, BaseUser)> getRefreshToken(string token)
        {
            var account = await _context.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null) throw new AppException("Invalid token");
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new AppException("Invalid token");
            return (refreshToken, account);
        }

        private string generateJwtToken(BaseUser account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.UserId.ToString()) }),
                Expires = GenericFunctions.GetCurrentDateTime().AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = randomTokenString(),
                Expires = GenericFunctions.GetCurrentDateTime().AddDays(7),
                Created = GenericFunctions.GetCurrentDateTime(),
                CreatedByIp = ipAddress
            };
        }

        private void removeOldRefreshTokens(BaseUser account)
        {
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= GenericFunctions.GetCurrentDateTime());
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public async Task<ServiceResponse<object>> SendContactEmail(ContactEmailDTO dto)
        {
            try
            {
                _emailService.SendContactEmail(dto);
                return new ServiceResponse<object> { Success=true };
            }catch(Exception e)
            {
                return new ServiceResponse<object> { Success = false };
            }
        }

        public async Task<ServiceResponse<IEnumerable<SimpleUserDTO>>> GetUsers()
        {
            try
            {
                var users = _context.Users.Where(x => x.UserType == UserType.SimpleUser && !x.IsDeleted).Select(x => new SimpleUserDTO
                {
                    UserId = x.UserId,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    CreatedAt = GenericFunctions.ParseNullableDateTimeAsDisplayableString(x.CreatedAt),
                    Updated = GenericFunctions.ParseNullableDateTimeAsDisplayableString(x.Updated),
                    NumarTelefon = (x as User).PhoneNumber,
                }).AsEnumerable();

                return new ServiceResponse<IEnumerable<SimpleUserDTO>> { Response = users, Success = true };
            }
            catch (Exception e)
            {
                return new ServiceResponse<IEnumerable<SimpleUserDTO>> { Success = false, Message = Messages.Message_UsersLoadError };
            }
        }
    }
}
