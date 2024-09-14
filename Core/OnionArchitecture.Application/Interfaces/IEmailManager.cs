namespace OnionArchitecture.Application.Interfaces;
public interface IEmailManager
{
    Task SendOtpAsync(string email, string otpCode);
}

