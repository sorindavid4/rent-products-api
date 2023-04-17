using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using rent_products_api.DataLayer.DTOs.Email;
using rent_products_api.DataLayer.DTOs.Rents;
using rent_products_api.DataLayer.Utils;
using rent_products_api.Models;
using rent_products_api.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;
        private Queue<EmailsInQueueDTO> _emailsQueue = new Queue<EmailsInQueueDTO>();

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            _emailsQueue.Enqueue(new EmailsInQueueDTO(email));
            // send emails
            if(_emailsQueue.Count == 1)
            {
                SendQueueEmails();
            }
           
        }
        public async void SendQueueEmails()
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
            while (_emailsQueue.Count > 0)
            {
                var emailQueue = _emailsQueue.Dequeue();
                emailQueue.TryCount--;
                try
                {
                    smtp.Send(emailQueue.Email);
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                    if (emailQueue.TryCount > 0)
                    {
                        _emailsQueue.Enqueue(emailQueue);
                    }
                    else
                    {
                    }

                }
            }
            smtp.Disconnect(true);

        }
        public async void ResendVerificationEmail(string email, string verificationToken)
        {
            string message;
            string origin = _appSettings.MailBaseUrl;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/verificare-email?token={verificationToken}";
                message = $@"<p>Vă rugăm apăsați pe link-ul de mai jos pentru a vă verifica email-ul:</p>
                             <p><a href=""{verifyUrl}"">Verifică email</a></p>";
            }
            else
            {
                message = $@"<p>Vă rugăm contactați un expert cu codul de mai jos pentru a verifica email-ul</p>
                             <p><code>{verificationToken}</code></p>";
            }
            var body = await GetMailTemplate("Verifică email-ul", $@"<p>Ați primit un nou cod de verificare pentru platforma ATV.</p>{message}");
            Send(
                to: email,
                subject: "ATV: Verifică Email",
                html: body
            );
        }
        public async void SendVerificationEmail(BaseUser account, string origin)
        {
            string message;
            origin = _appSettings.MailBaseUrl;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/verifica-email?token={account.VerificationToken}";
                message = $@"<p>Vă rugăm apăsați pe link-ul de mai jos pentru a vă verifica email-ul:</p>
                             <p><a href=""{verifyUrl}"">Verifică email</a></p>";
            }
            else
            {
                message = $@"<p>Vă rugăm contactați un expert cu codul de mai jos pentru a verifica email-ul</p>
                             <p><code>{account.VerificationToken}</code></p>";
            }
            var body =await GetMailTemplate("Verifică email-ul", $@"<p>Ați fost înregistrat cu succes pe platforma ATV</p>{message}");
            Send(
                to: account.Email,
                subject: "ATV: Verifică Email",
                html: body
            );
        }
        public async void SendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            origin = _appSettings.MailBaseUrl;
            var alreadyRegisteredUrl = $"{origin}/users/resetPassword";
            message = $@"<p>Dacă ați uitat parola folositi link-ul următor pentru a o reseta <a href=""{alreadyRegisteredUrl}"">Link</a></p>";
            var html =await GetMailTemplate("Cont existent", $@"<p>Email-ul <strong>{email}</strong> este deja înregistrat.</p>{ message}");
            Send(
                to: email,
                subject: "Acest email este deja înregistrat în platformă",
                html: html
            );
        }
        public async void SendPasswordResetEmail(BaseUser account, string origin)
        {
            string message;
            origin = _appSettings.MailBaseUrl;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/users/ResetPassword?token={account.ResetToken}";
                message = $@"<p>Apăsați pe link-ul de mai jos pentru a reseta parola. Link-ul e valabil 24 de ore de la primire</p>
                             <p><a href=""{resetUrl}"">Resetează parola</a></p>";
            }
            else
            {
                message = $@"<p>Vă rugăm contactați un expert din cadrul proiectului cu token-ul de mai jos, pentru resetarea parolei</p>
                             <p><code>{account.ResetToken}</code></p>";
            }
            var html =await GetMailTemplate("Verificare resetare parola", $@"{message}");
            Send(
                to: account.Email,
                subject: "Verificare resetare parola",
                html: html
            );
        }

        private async Task<string> GetMailTemplate(string title, string content)
        {
            string mailHtml = $@"<!DOCTYPE html>
                        <html>
                        <body style=""text-align:center ;padding: 20px;"">
                         <div>
                            <div style=""display:flex; justify-content:center;"">
                            <div style=""width:50%;"">
                            <img height=""100"" max-width=""25%"" src="""" />
                            </div>
                        
                            <img height=""100"" max-width=""25%"" src="""" />
                            </div>
                            <div style=""text-align:center;"">
                               <h1> { title} </h1>
                            <div style=""padding-bottom: 20px; font-size:1.8em "" > { content } </div>
                            </div>
                            <div style=""text-align:center; margin-top:15px; display:flex;"">
                            <div style=""width:50%"">
                            <img style=""margin: 1px;"" max-width=""25%"" height=""100"" src="""" />
                            </div>
                            <div style=""width:50%"">
                            <img style=""margin: 1px;"" max-width=""25%"" height=""100"" src="""" />
                            </div> 
                            </div>
                         </div >
                        </body>
                        </html> ";

            return mailHtml;
        }

        public async void SendWantedRentEmail(EmailRentDetails details)
        {
            string message="";
            var origin = _appSettings.MailBaseUrl;
            var url = $"{origin}/inchirieri";
            if(details.RentType == RentType.WholeDay)
            {
                message = $@"<p>{details.Username} vrea să închirieze {details.ATVname} în data de {details.RentDate.ToString("dd.MM.yyyy")}.</p>";
            }
            if(details.RentType == RentType.FewHours)
            {
                message = $@"<p>{details.Username} vrea să închirieze {details.ATVname} în data de {details.RentDate.ToString("dd.MM.yyyy")} de la ora
                {GenericFunctions.ParseTimeSpanToString(details.StartingHour)} până la {GenericFunctions.ParseTimeSpanToString(details.EndingHour)}.</p>";
            }
            var bottomMessage = $@"<p>Link platformă <a href=""{url}"">Link</a></p>";
            var html = await GetMailTemplate($@"{details.Username} vrea să închirieze!", $@"<p>{ message}</p>{bottomMessage}");
            Send(
                to: _appSettings.SmtpUser,
                subject: "Platformă închirieri",
                html: html
            );
        }

        public async void SendRentConfirmedEmail(EmailRentDetails details)
        {
            string message = "";
            var origin = _appSettings.MailBaseUrl;
            var url = $"{origin}/inchirieri";
            if (details.RentType == RentType.WholeDay)
            {
                message = $@"<p>A fost confirmată închirierea pentru {details.ATVname} în data de {details.RentDate.ToString("dd.MM.yyyy")}.</p>";
            }
            if (details.RentType == RentType.FewHours)
            {
                message = $@"<p>A fost confirmată închirierea pentru {details.ATVname} în data de {details.RentDate.ToString("dd.MM.yyyy")} de la ora
                {GenericFunctions.ParseTimeSpanToString(details.StartingHour)} până la {GenericFunctions.ParseTimeSpanToString(details.EndingHour)}.</p>";
            }
            var bottomMessage = $@"<p>Link platformă <a href=""{url}"">Link</a></p>";
            var html = await GetMailTemplate($@"Confirmare închiriere!", $@"<p>{ message}</p>{bottomMessage}");
            Send(
                to: details.Email,
                subject: "Platformă închirieri",
                html: html
            );
        }

        public async void SendRentRejectedEmail( EmailRentDetails details)
        {
            string message = "";
            var origin = _appSettings.MailBaseUrl;
            var url = $"{origin}/inchirieri";
            if (details.RentType == RentType.WholeDay)
            {
                message = $@"<p>A fost respinsă închirierea pentru {details.ATVname} în data de {details.RentDate.ToString("dd.MM.yyyy")}.</p>";
            }
            if (details.RentType == RentType.FewHours)
            {
                message = $@"<p>A fost respinsă închirierea pentru {details.ATVname} în data de {details.RentDate.ToString("dd.MM.yyyy")} de la ora
                {GenericFunctions.ParseTimeSpanToString(details.StartingHour)} până la {GenericFunctions.ParseTimeSpanToString(details.EndingHour)}.</p>";
            }
            var bottomMessage = $@"<p>Link platformă <a href=""{url}"">Link</a></p>";
            var html = await GetMailTemplate($@"Închiriere respinsă!", $@"<p>{ message}</p>{bottomMessage}");
            Send(
                to: details.Email,
                subject: "Platformă închirieri",
                html: html
            );
        }

        public async void SendPaymentConfirmedEmail(EmailRentDetails details)
        {
            string message = "";
            var origin = _appSettings.MailBaseUrl;
            var url = $"{origin}/inchirieri";
            if (details.RentType == RentType.WholeDay)
            {
                message = $@"<p>A fost plătită închirierea de către {details.Username} pentru {details.ATVname} în data de {details.RentDate.ToString("dd.MM.yyyy")}.</p>";
            }
            if (details.RentType == RentType.FewHours)
            {
                message = $@"<p>A fost plătită închirierea de către {details.Username} pentru {details.ATVname} în data de {details.RentDate.ToString("dd.MM.yyyy")} de la ora
                {GenericFunctions.ParseTimeSpanToString(details.StartingHour)} până la {GenericFunctions.ParseTimeSpanToString(details.EndingHour)}.</p>";
            }
            var bottomMessage = $@"<p>Link platformă <a href=""{url}"">Link</a></p>";
            var html = await GetMailTemplate($@"Plată confirmată!", $@"<p>{ message}</p>{bottomMessage}");
            Send(
                to: _appSettings.SmtpUser,
                subject: "Platformă închirieri",
                html: html
            );
        }

        public async void SendContactEmail(ContactEmailDTO emailDTO)
        {
            string message;
            string origin = _appSettings.MailBaseUrl;
            var rentDate = GenericFunctions.ParseStringToDateTime(emailDTO.RentDate);
            var date = GenericFunctions.ParseNullableDateTimeAsDisplayableString(rentDate);
            message = $@"<h3>Email expeditor: {emailDTO.Email}</h3>
                        <p>{emailDTO.Message}</p>
                        <p>Perioada dorită: {emailDTO.RentPeriod}</p>
                        <p>Tip de ATV dorit: {emailDTO.AtvWanted}</p>
                        <p>Link platformă <a href=""{origin}"">Link</a></p>";
            var html = await GetMailTemplate($@"Număr telefon:{emailDTO.PhoneNumber}", message);
            Send(
                to: _appSettings.SmtpUser,
                subject: $@"{emailDTO.Name} vrea să vă contacteze.",
                html: html
            );
        }
    }
}
