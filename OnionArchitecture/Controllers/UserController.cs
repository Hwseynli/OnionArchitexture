using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Commands.User.ForgotPassword;
using OnionArchitecture.Application.Features.Commands.User.Login;
using OnionArchitecture.Application.Features.Commands.User.RefreshToken;
using OnionArchitecture.Application.Features.Commands.User.Register;
using OnionArchitecture.Application.Features.Commands.User.UpdatePasswordWithOtp;
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
    public async Task<IActionResult> GetProfile([FromQuery] string email)
    {
        var profile = await _userQueries.GetUserProfileAsync(email);
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

    [HttpPost("resetPassword")]
    public async Task<IActionResult> UpdatePasswordWithOtp(UpdatePasswordWithOtpCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Password updated successfully.") : BadRequest("Invalid OTP or OTP has expired.");
    }
}