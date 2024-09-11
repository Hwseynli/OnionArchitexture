using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitecture.Application.Features.Commands.User.ViewModels
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken{ get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
