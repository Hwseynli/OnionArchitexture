using MediatR;
using OnionArchitecture.Application.Features.Commands.User.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitecture.Application.Features.Commands.User.RefreshToken
{
    public class RefreshTokenCommand:IRequest<JwtTokenDto>
    {
        public string RefreshToken {  get; set; }
    }
}
