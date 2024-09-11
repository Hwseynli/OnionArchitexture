using MediatR;
using OnionArchitecture.Application.Features.Commands.User.ViewModels;

namespace OnionArchitecture.Application.Features.Commands.User.Login
{
    public class CreateAuthorizationTokenCommand:IRequest<JwtTokenDto>
    {
        public string Username {  get; set; }
        public string Password { get; set; }    
    }
}
