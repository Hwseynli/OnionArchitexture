using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Commands.User.ForgotPassword.ResetPassword;
using OnionArchitecture.Application.Features.Commands.User.ForgotPassword.SendOtp;
using OnionArchitecture.Application.Features.Commands.User.ForgotPassword.VerifyOtp;
using OnionArchitecture.Application.Features.Commands.User.Login;
using OnionArchitecture.Application.Features.Commands.User.RefreshToken;
using OnionArchitecture.Application.Features.Commands.User.Register;
using OnionArchitecture.Application.Features.Commands.User.UpdateUser;
using OnionArchitecture.Application.Features.Queries;

namespace OnionArchitecture.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserQueries _userQueries;

    public UserController(IMediator mediator, IUserQueries userQueries)
    {
        _mediator = mediator;
        _userQueries = userQueries;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(CreateAuthorizationTokenCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("User updated successfully.") : BadRequest("User not found or invalid data.");
    }


    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _userQueries.GetUserProfileAsync();
        return Ok(profile);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("sendOtp")]
    public async Task<IActionResult> SendOtp(SendOtpCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("OTP has been sent to your email.") : BadRequest("User not found");
    }

    [HttpPost("verifyOtp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpCommand command)
    {
        var isValid = await _mediator.Send(command);
        return isValid ? Ok("OTP is valid.") : BadRequest("Invalid or expired OTP.");
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        var isReset = await _mediator.Send(command);
        return isReset ? Ok("Password updated successfully.") : BadRequest("Failed to reset password.");
    }
}