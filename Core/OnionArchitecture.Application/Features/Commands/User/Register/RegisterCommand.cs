using MediatR;

namespace OnionArchitecture.Application.Features.Commands.User.Register;
public class RegisterCommand:IRequest<bool>
{
    public string Name {  get; set; }
    public string Surname {  get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
}
