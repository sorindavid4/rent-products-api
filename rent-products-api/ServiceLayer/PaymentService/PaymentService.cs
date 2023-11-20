using AutoMapper;
using Microsoft.Extensions.Options;
using MobilpayEncryptDecrypt;
using rent_products_api.DataLayer.DTOs.Payments;
using rent_products_api.DataLayer.Models.Payments;
using rent_products_api.DataLayer.Utils;
using rent_products_api.DBContexts;
using rent_products_api.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.PaymentService
{
    public class PaymentService:IPaymentService
    {
        private readonly MainDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public PaymentService(MainDbContext dbContext, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _context = dbContext;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        public async Task<ServiceResponse<RequestPaymentDTO>> GoToPayment(GoToPaymentDTO paymentDTO, string origin)
        {
            try
            {
                var userType = _context.Users.Where(x => x.UserId == paymentDTO.UserId).Select(x => x.UserType).FirstOrDefault();
                var paymentId = _context.Rents.Where(x => x.RentId == paymentDTO.RentId).Select(x => x.PaymentId).FirstOrDefault();
                MobilpayEncrypt encrypt = new MobilpayEncrypt();

                Mobilpay_Payment_Request_Card card = new Mobilpay_Payment_Request_Card();
                Mobilpay_Payment_Invoice invoice = new Mobilpay_Payment_Invoice();
                Mobilpay_Payment_Address billing = new Mobilpay_Payment_Address();
                Mobilpay_Payment_Request_Contact_Info ctinfo = new Mobilpay_Payment_Request_Contact_Info();
                Mobilpay_Payment_Request_Url url = new Mobilpay_Payment_Request_Url();
                Mobilpay_Payment_Params userId = new Mobilpay_Payment_Params();
                Mobilpay_Payment_Params paymentType = new Mobilpay_Payment_Params();

                userId.Name = "userId";
                userId.Value = paymentDTO.UserId.ToString();

                paymentType.Name = "paymentType";
                paymentType.Value = paymentDTO.PaymentType.ToString();

                MobilpayEncryptDecrypt.MobilpayEncryptDecrypt encdecr = new MobilpayEncryptDecrypt.MobilpayEncryptDecrypt();

                card.OrderId = paymentId.ToString();
                card.Type = "card";
                card.Signature = _appSettings.NetopiaSignature;
                card.Params = new Mobilpay_Payment_ParamsCollection();

                card.Params.Add(userId);
                card.Params.Add(paymentType);

                url.ConfirmUrl = origin + "/Payment/AddOrUpdatePayment";
                url.ReturnUrl = _appSettings.MailBaseUrl + "/plata";

                card.Service = "";
                card.Url = url;
                card.TimeStamp = GenericFunctions.GetCurrentDateTime().ToString("yyyyMMddhhmmss");

                invoice.Amount = 300;
                invoice.Currency = "RON";
                invoice.Details = "Rezervare ATV";
                billing.Type = "person";
                billing.Email = paymentDTO.Email;

                ctinfo.Billing = billing;

                invoice.ContactInfo = ctinfo;

                card.Invoice = invoice;
                encrypt.Data = encdecr.GetXmlText(card);
                encrypt.X509CertificateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\public.cer");
                encdecr.Encrypt(encrypt);
                System.Collections.Specialized.NameValueCollection coll = new System.Collections.Specialized.NameValueCollection();
                coll.Add("data", encrypt.EncryptedData);
                coll.Add("env_key", encrypt.EnvelopeKey);

                var response = new RequestPaymentDTO
                {
                    Env_Key = coll["env_key"],
                    Data = coll["data"],
                    Url = _appSettings.NetopiaUrl,
                    PaymentType = paymentDTO.PaymentType
                };

                return new ServiceResponse<RequestPaymentDTO> { Response = response, Success = true };
            }
            catch (Exception e)
            {

                return new ServiceResponse<RequestPaymentDTO> { Success = false,  };
            }
        }
        public async Task<ServiceResponse<object>> AddOrUpdatePayment(string env_key, string data)
        {
            try
            {
                MobilpayEncryptDecrypt.MobilpayEncryptDecrypt encdecrypt = new MobilpayEncryptDecrypt.MobilpayEncryptDecrypt();
                MobilpayDecrypt decrypt = new MobilpayDecrypt();
                decrypt.Data = data;
                decrypt.EnvelopeKey = env_key;
                decrypt.PrivateKeyFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\private.key");
                decrypt.X509CertificateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\public.cer");

                var card = encdecrypt.BuildCardDecrypt(decrypt);


                var transactionId = Guid.Parse(card.OrderId);
                var transactionStatus = card.Confirm.Action;
                var amount = card.Confirm.Original_Amount;
                var userId = Guid.Parse(card.Params[0].Value);
                var paymentType = Enum.Parse<PaymentType>(card.Params[1].Value);


                var paymentLog = _context.Payments.FirstOrDefault(x => x.PaymentId == transactionId);

                if (paymentLog != null)
                {
                    //paymentLog.StatusUpdateTime = GenericFunctions.GetCurrentDateTime();
                    //paymentLog.TransactionStatus = transactionStatus;
                    //paymentLog.Amount = amount;
                    //paymentLog.PaymentType = paymentType;

                }
                else
                {
                    paymentLog = new Payment
                    {
                        
                    };
                    _context.Payments.Add(paymentLog);
                }

                _context.SaveChanges();

                return new ServiceResponse<object> { Success = true };
            }
            catch (Exception e)
            {
      
                return new ServiceResponse<object> { Success = false };
            }
        }
        public async Task<ServiceResponse<List<PaymentDTO>>> GetPayments(Guid userId)
        {
            try
            {
                var userType = _context.Users.Where(x=>x.UserId== userId).Select(x=>x.UserType).FirstOrDefault();
                var payments = new List<PaymentDTO>();
                if (userType == UserType.SimpleUser)
                {
                    payments = _mapper.ProjectTo<PaymentDTO>(_context.Rents.Where(x => x.RentedByUserId == userId && x.PaymentType == PaymentType.Online)).ToList();
                }
                else
                {
                    payments = _mapper.ProjectTo<PaymentDTO>(_context.Rents.Where(x => x.PaymentType == PaymentType.Online)).ToList();
                }

                return new ServiceResponse<List<PaymentDTO>> { Response = payments, Success = true };

            }catch(Exception e)
            {
                return new ServiceResponse<List<PaymentDTO>> { Success =false };
            }
        }
    }
}
