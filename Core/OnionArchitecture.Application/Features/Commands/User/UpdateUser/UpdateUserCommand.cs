﻿using MediatR;

namespace OnionArchitecture.Application.Features.Commands.User.UpdateUser;
public class UpdateUserCommand : IRequest<bool>
{
    public int Id { get; set; } // Mövcud istifadəçini müəyyən etmək üçün
    public string Email { get; set; }  
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }  // Mövcud parolun yoxlanılması üçün
    public string UserName { get; set; }  // Username yeniləmək istəyirsinizsə
}

