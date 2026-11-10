using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop.ApplicationServices.Services
{
    public class EmailsServices : IEmailsServices
    {
        private readonly EmailSettings _settings;

        public EmailsServices(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public void SendEmail(EmailDto dto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.UserName));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };

            if (dto.Attachment != null)
            {
                foreach (var file in dto.Attachment)
                {
                    if (file.Length > 0 && file.Length < 10485760) // 10mb
                    {
                        using var stream = new MemoryStream();
                        file.CopyTo(stream);
                        stream.Position = 0; // Reset stream position
                        builder.Attachments.Add(file.FileName, stream.ToArray());
                    }
                }
            }

            email.Body = builder.ToMessageBody();
            SendSmtp(email);
        }

        public void SendEmailToken(EmailTokenDto dto, string token)
        {
            dto.Token = token;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.UserName));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body,
            };

            email.Body = builder.ToMessageBody();
            SendSmtp(email);
        }

        private void SendSmtp(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            var secureOption = _settings.Port == 465
                ? MailKit.Security.SecureSocketOptions.SslOnConnect
                : MailKit.Security.SecureSocketOptions.StartTls;

            smtp.Connect(_settings.Host, _settings.Port, secureOption);
            smtp.Authenticate(_settings.UserName, _settings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
