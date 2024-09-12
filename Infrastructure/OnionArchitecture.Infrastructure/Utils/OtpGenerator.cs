namespace OnionArchitecture.Infrastructure.Utils;
public static class OtpGenerator
{
    public static string GenerateOtp()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString(); // 6 rəqəmli OTP yaradılır
    }
}

