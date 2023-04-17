using AutoMapper;
using rent_products_api.DataLayer.DTOs.Email;
using rent_products_api.DataLayer.DTOs.Product;
using rent_products_api.DataLayer.DTOs.Rents;
using rent_products_api.DataLayer.Models;
using rent_products_api.DataLayer.Models.Payments;
using rent_products_api.DataLayer.Utils;
using rent_products_api.DBContexts;
using rent_products_api.ServiceLayer.EmailService;
using rent_products_api.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.RentService
{
    public class RentService:IRentService
    {
        private readonly MainDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public RentService(MainDbContext context, IMapper mapper,IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<ServiceResponse<object>> CancelRent(Guid rentId)
        {
            try
            {
                var rent = _context.Rents.FirstOrDefault(x => x.RentId == rentId && DateTime.Compare(DateTime.Now, x.RentedDate) < 0);
                if (rent == null)
                {
                    return new ServiceResponse<object> { Success = false };
                }
                rent.Status = RentStatus.Rejected;
                rent.RejectionTime = GenericFunctions.GetCurrentDateTime();
                _context.SaveChanges();
                try
                {
                    var rentDetails = _context.Rents.Where(x => x.RentId == rentId).Select(x => new EmailRentDetails
                    {
                        Email = x.RentedByUser.Email,
                        Username = x.RentedByUser.LastName + " " + x.RentedByUser.FirstName,
                        ATVname = x.Product.Name,
                        StartingHour = x.StartingHour,
                        EndingHour = x.EndingHour,
                        RentDate = x.RentedDate,
                        RentType = x.RentType,
                    }).FirstOrDefault();
                    if (rentDetails != null)
                    {
                        _emailService.SendRentRejectedEmail(rentDetails);
                    }
                }
                catch (Exception)
                {

                }
                return new ServiceResponse<object> { Success = true };
            }
            catch(Exception e)
            {
                return new ServiceResponse<object> { Success = false };
            }
        }

        public async Task<ServiceResponse<object>> ConfirmRent(Guid rentId)
        {
            try
            {
                var rent = _context.Rents.FirstOrDefault(x => x.RentId == rentId && DateTime.Compare(DateTime.Now, x.RentedDate) < 0);
                if (rent == null)
                {
                    return new ServiceResponse<object> { Success = false };
                }
                if (rent.PaymentType == PaymentType.Online)
                {
                    rent.Status = RentStatus.WaitingForOnlinePayment;
                }else if (rent.PaymentType == PaymentType.Cash)
                {
                    rent.Status = RentStatus.Confirmed;
                }
                rent.ConfirmationTime = GenericFunctions.GetCurrentDateTime();
                _context.SaveChanges();

                try
                {
                        var rentDetails = _context.Rents.Where(x => x.RentId == rentId).Select(x => new EmailRentDetails
                        {
                            Email = x.RentedByUser.Email,
                            Username = x.RentedByUser.LastName + " " + x.RentedByUser.FirstName,
                            ATVname = x.Product.Name,
                            StartingHour = x.StartingHour,
                            EndingHour = x.EndingHour,
                            RentDate = x.RentedDate,
                            RentType = x.RentType,
                        }).FirstOrDefault();
                        if (rentDetails != null)
                        {
                            _emailService.SendRentConfirmedEmail(rentDetails);
                        }
                }
                catch (Exception)
                {

                }
                return new ServiceResponse<object> { Success = true };
            }
            catch (Exception e)
            {
                return new ServiceResponse<object> { Success = false };
            }
        }

        public async Task<ServiceResponse<object>> ConfirmRentPayment(Guid rentId)
        {
            try
            {
                var payment = _context.Rents.Where(x=>x.RentId==rentId && DateTime.Compare(DateTime.Now,x.RentedDate)<0).Select(x=>x.Payment).FirstOrDefault();
                if (payment == null)
                {
                    return new ServiceResponse<object> { Success = false };
                }
                payment.PaymentConfirmed = true;
                _context.SaveChanges();
                try
                {
                    var rentDetails = _context.Rents.Where(x => x.RentId == rentId).Select(x => new EmailRentDetails
                    {
                        Email = x.RentedByUser.Email,
                        Username = x.RentedByUser.LastName + " " + x.RentedByUser.FirstName,
                        ATVname = x.Product.Name,
                        StartingHour = x.StartingHour,
                        EndingHour = x.EndingHour,
                        RentDate = x.RentedDate,
                        RentType = x.RentType,
                    }).FirstOrDefault();
                    if (rentDetails != null)
                    {
                        _emailService.SendPaymentConfirmedEmail(rentDetails);
                    }
                }
                catch (Exception)
                {

                }
                return new ServiceResponse<object> { Success = true };
            }
            catch (Exception e)
            {
                return new ServiceResponse<object> { Success = false };
            }
        }

        public async Task<ServiceResponse<List<RentDTO>>> GetMyRents(Guid userId)
        {
            try
            {
                var userType = _context.Users.Where(x => x.UserId == userId).Select(x => x.UserType).FirstOrDefault();
                var rents = new List<RentDTO>();

                if (userType == UserType.AdminUser)
                {
                      rents = _mapper.ProjectTo<RentDTO>(_context.Rents.Where(x=>DateTime.Compare(DateTime.Now,x.RentedDate)<0).OrderBy(x=>x.RentedDate)).ToList();
                }
                else
                {
                    rents = _mapper.ProjectTo<RentDTO>(_context.Rents.Where(x => x.RentedByUserId == userId).OrderBy(x=>x.RentedDate)).ToList();
                }
                return new ServiceResponse<List<RentDTO>> { Response = rents, Success = true };
            }
            catch (Exception e)
            {
                return new ServiceResponse<List<RentDTO>> { Success = false };
            }
        }

        public async Task<ServiceResponse<List<ProductRentedTime>>> GetProductRentedTimes(Guid productId)
        {
            try
            {
                var rents = _mapper.ProjectTo<ProductRentedTime>(_context.Rents.Where(x =>x.Status!=RentStatus.Rejected && x.ProductId == productId && DateTime.Compare(x.RentedDate,GenericFunctions.GetCurrentDateTime())>=0)).ToList();

                return new ServiceResponse<List<ProductRentedTime>> {Response=rents, Success = true };
            }
            catch (Exception e)
            {
                return new ServiceResponse<List<ProductRentedTime>> { Success = false };
            }
        }

        public async Task<ServiceResponse<object>> RentProduct(RentProductDTO rentDTO)
        {
            try
            {
                if (rentDTO.RentedByUserId == Guid.Empty)
                {
                    return new ServiceResponse<object> { Success = false, Message = Messages.Message_UserNotFound };
                }
                if(rentDTO.ProductId == Guid.Empty)
                {
                    return new ServiceResponse<object> { Success = false, Message = Messages.Message_GetProductsError };
                }

                var rent = _mapper.Map<Rent>(rentDTO);
                rent.Status = RentStatus.Rented;
                var productPrices = _mapper.ProjectTo<ProductDTO>(_context.Products.Where(x => x.ProductId == rentDTO.ProductId)).FirstOrDefault();
                _context.Rents.Add(rent);
                if (rentDTO.PaymentType == PaymentType.Online)
                {
                    var payment = new Payment
                    {
                        Amount = rentDTO.RentType == RentType.FewHours ? getPriceByHours(rent.StartingHour, rent.EndingHour, productPrices.PricePerHour) : productPrices.PricePerDay,
                        PaymentConfirmed = false,
                        UserPayingId = rentDTO.RentedByUserId
                    };
                    _context.Payments.Add(payment);
                    rent.PaymentId = payment.PaymentId;
                }
                
                var saved = await _context.SaveChangesAsync();

                try
                {
                    if (saved>0)
                    {
                        var rentDetails = _context.Rents.Where(x => x.RentId == rent.RentId).Select(x => new EmailRentDetails
                        {
                            ATVname = x.Product.Name,
                            StartingHour = x.StartingHour,
                            EndingHour = x.EndingHour,
                            RentDate = x.RentedDate,
                            RentType = x.RentType,
                        }).FirstOrDefault();
                        rentDetails.Username = rentDTO.Username;
                        if (rentDetails != null)
                        {
                            _emailService.SendWantedRentEmail(rentDetails);
                        }
                    }
                    
                }
                catch (Exception)
                {

                }

                return new ServiceResponse<object> { Success = true, Message=Messages.Message_RentProductSuccess };
            }
            catch (Exception e)
            {
                return new ServiceResponse<object> { Success = false, Message = Messages.Message_RentProductError };
            }
        }

        public async Task<ServiceResponse<object>> UncancelRent(Guid rentId)
        {
            try
            {
                var rent = _context.Rents.FirstOrDefault(x => x.RentId == rentId && DateTime.Compare(DateTime.Now, x.RentedDate) < 0);
                if (rent == null) {
                    return new ServiceResponse<object> { Success = false };
                }
                rent.Status = RentStatus.Rented;
                rent.RejectionTime = DateTime.MinValue;
                _context.SaveChanges();

                return new ServiceResponse<object> { Success = true };
            }
            catch (Exception e)
            {
                return new ServiceResponse<object> { Success = false };
            }
        }

        public async Task<ServiceResponse<object>> UnconfirmRent(Guid rentId)
        {
            try
            {
                var rent = _context.Rents.FirstOrDefault(x => x.RentId == rentId && DateTime.Compare(DateTime.Now, x.RentedDate) < 0);
                if(rent == null)
                {
                    return new ServiceResponse<object> { Success = false };
                }
                rent.Status = RentStatus.Rented;
                rent.ConfirmationTime = DateTime.MinValue;
                _context.SaveChanges();

                return new ServiceResponse<object> { Success = true };
            }
            catch (Exception e)
            {
                return new ServiceResponse<object> { Success = false };
            }
        }

        private int getPriceByHours(TimeSpan startingHour, TimeSpan endingHour, int pricePerHour)
        {
            var timeBetween = endingHour - startingHour;
            var amount = timeBetween.TotalHours * pricePerHour;
            return Convert.ToInt32(amount);
        }
    }
}
