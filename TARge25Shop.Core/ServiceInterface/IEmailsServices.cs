using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IEmailsServices
    {
        void SendEmail(EmailDto dto);
        void SendEmailToken(EmailTokenDto dto, string token);
    }
}