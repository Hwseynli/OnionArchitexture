using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArchitecture.Application.Features.Commands.User.Login
{
    public class CreateAuthorizationTokenCommandValidator:AbstractValidator<CreateAuthorizationTokenCommand>
    {
        public CreateAuthorizationTokenCommandValidator()
        {
            RuleFor(command => command.Username).NotNull();
            RuleFor(command => command.Password).MinimumLength(10).NotNull();
        }
    }
}
