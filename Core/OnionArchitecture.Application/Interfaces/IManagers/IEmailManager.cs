namespace OnionArchitecture.Application.Interfaces.IManagers;
public interface IEmailManager
{
    Task SendOtpAsync(string email, string otpCode);
}

