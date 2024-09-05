using System;
using MediatR;

namespace OnionArchitecture.Application.Features.Commands.User.Register;
public class RegisterCommand:IRequest<bool>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

