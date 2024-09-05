using System;
using OnionArchitecture.Domain.Entities;

namespace OnionArchitecture.Domain;
public class Editable<TUser> : Auditable<TUser> where TUser : User
{
    public int? UpdatedById { get; protected set; }
    public DateTime? LastUpdateDateTime { get; protected set; }

    public void SetEditFields(int? updatedById)
    {
        UpdatedById = updatedById;
        LastUpdateDateTime = DateTime.UtcNow.AddHours(4);
    }
}


