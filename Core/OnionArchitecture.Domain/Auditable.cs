﻿using OnionArchitecture.Domain.Common;
using OnionArchitecture.Domain.Entities;
using OnionArchitecture.Domain.Exceptions;

namespace OnionArchitecture.Domain;
public class Auditable<TUser>:IBaseEntity where TUser : User
{
    public int Id { get; set; }
    public int CreatedById { get;protected set; }
    public DateTime RecordDateTime { get;protected set; }

    public void SetAuditDetails(int createdById)
    {
        if(CreatedById != 0 && CreatedById != createdById)
        {
            throw new DomainException("CreatedBy already set.");
        } 
        CreatedById = createdById;
        RecordDateTime = DateTime.UtcNow.AddHours(4);
    }
}
