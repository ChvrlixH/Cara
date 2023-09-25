using Cara.Business.DTOs;

namespace Cara.Business.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(MailRequestDto mailRequest);
}
