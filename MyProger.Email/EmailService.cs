using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace MyProger.Email;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        //System.Net.Mail.SmtpClient
        public void SendEmailDefault()
        {
            try
            {
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true; //тело сообщения в формате HTML
                message.From = new MailAddress("Ponomareff.55555@gmail.com", "DotNetLearning"); //отправитель сообщения
                message.To.Add("mail@yandex.ru"); //адресат сообщения
                message.Subject = "Message from DotNetLearning"; //тема сообщения
                message.Body = $"<div style=\"color: red;\">Message from DotNetLearning</div>"; //тело сообщения
                //! message.Attachments.Add(new Attachment()); //добавить вложение к письму при необходимости

                using SmtpClient client = new SmtpClient("smtp.gmail.com");
                
                client.Credentials = new NetworkCredential("mail@gmail.com", "secret"); //логин-пароль от аккаунта
                client.Port = 587; //порт 587 либо 465
                client.EnableSsl = true; //SSL обязательно

                client.Send(message);
                _logger.LogInformation("Сообщение отправлено успешно!");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.GetBaseException().Message);
            }
        }

        //MailKit.Net.Smtp.SmtpClient
        public void SendEmailCustom(string name, string address, string emialMessage)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("DotNetLearning", "Ponomareff.55555@gmail.com")); //отправитель сообщения
                message.To.Add(new MailboxAddress(name, address)); //адресат сообщения
                message.Subject = "Message from DotNetLearning"; //тема сообщения
                message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color: green;\">{emialMessage}</div>" }.ToMessageBody(); //тело сообщения (так же в формате HTML)

                //using MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                //
                //client.Connect("smtp.gmail.com", 465, true); //либо использум порт 465
                //client.Authenticate("Laven So2", "Sasha_2008!"); //логин-пароль от аккаунта
                //client.Send(message);
//
                //client.Disconnect(true);
                //_logger.LogInformation("Сообщение отправлено успешно!");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.GetBaseException().Message);
            }
        }
    }