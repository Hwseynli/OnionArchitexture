namespace OnionArchitecture.Infrastructure.Services;
public interface IEmailService
{
    Task SendOtpAsync(string email, string otpCode);
}

