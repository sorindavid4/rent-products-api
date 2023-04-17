using rent_products_api.DataLayer.DTOs.Email;
using rent_products_api.DataLayer.DTOs.Rents;
using rent_products_api.DataLayer.DTOs.User;
using rent_products_api.DataLayer.Utils;
using rent_products_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.EmailService
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
        void SendVerificationEmail(BaseUser account, string origin);
        void SendPasswordResetEmail(BaseUser account, string origin);
        void SendAlreadyRegisteredEmail(string email, string origin);

        void SendWantedRentEmail(EmailRentDetails rentDetails);
        void SendRentConfirmedEmail(EmailRentDetails rentDetails);
        void SendRentRejectedEmail(EmailRentDetails rentDetails);
        void SendPaymentConfirmedEmail( EmailRentDetails rentDetails);
        void SendContactEmail(ContactEmailDTO dto);
        void ResendVerificationEmail(string email, string verificationToken);
    }
}
