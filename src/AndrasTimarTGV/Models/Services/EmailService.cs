using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AndrasTimarTGV.Models.Entities;
using SendGrid;

namespace AndrasTimarTGV.Models.Services
{
    public class EmailService : IEmailService
    {
        private const string EmailTemplateHtmlFile = "email_template.html";
        private const string EnvSengridApiKey = "SENDGRID_API_KEY";

        public void SendMail(Reservation reservation)
        {
            var recipient = reservation.User;
            var message = new SendGridMessage
            {
                From = new MailAddress("TGVSchool@gmail.com", "TGV mail service"),
                Subject = "Reservation Confirmation"
            };
            message.AddTo(recipient.Email);
            try
            {
                message.Html = File.ReadAllText(EmailTemplateHtmlFile)
                    .Replace("{{NAME}}", recipient.FirstName + " " + recipient.LastName)
                    .Replace("{{TRIP_SUMMARY}}", reservation.Trip.ToString());

                var apiKey = Environment.GetEnvironmentVariable(EnvSengridApiKey);
                if (apiKey != null)
                {
                    var client = new Web(apiKey);

                    client.DeliverAsync(message).Wait();
                }
            }
            catch (FileNotFoundException e)
            {
            }
        }
    }
}