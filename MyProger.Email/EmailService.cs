using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using MimeKit;
using MyProger.Core.Entity.Mail;

namespace MyProger.Email;

public class EmailService
{
    private readonly SmtpClient _smtpClient;

        public EmailService(string smtpServer,
            int smtpPort,
            string smtpUsername,
            string smtpPassword)
        {
            _smtpClient = new SmtpClient(smtpServer, smtpPort);
            _smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        }

        //System.Net.Mail.SmtpClient
        public void SendEmailDefault(MailMessageEntity mailMessage)
        {
            try
            {
                MailMessage message = new MailMessage()
                {
                    Body = mailMessage.Body,
                    Subject = mailMessage.Subject,
                };
                message.To.Add(mailMessage.To.FirstOrDefault()!);

            _smtpClient.Send(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }