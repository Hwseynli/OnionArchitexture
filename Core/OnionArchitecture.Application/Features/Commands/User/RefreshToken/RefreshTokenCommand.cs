using MediatR;
using OnionArchitecture.Application.Features.Commands.User.ViewModels;

namespace OnionArchitecture.Application.Features.Commands.User.RefreshToken;
public class RefreshTokenCommand:IRequest<JwtTokenDto>
{
    public string RefreshToken {  get; set; }
}
