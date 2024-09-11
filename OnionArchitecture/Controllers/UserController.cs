using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionArchitecture.Application.Features.Commands.User.Login;
using OnionArchitecture.Application.Features.Commands.User.RefreshToken;
using OnionArchitecture.Application.Features.Commands.User.Register;

namespace OnionArchitecture.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
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
            return Ok ( await _mediator.Send(command));
        } 
        
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            return Ok ( await _mediator.Send(command));
        }
    }
}
