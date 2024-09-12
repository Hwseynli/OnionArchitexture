using OnionArchitecture.Domain.Common;
using OnionArchitecture.Domain.Entities;
using OnionArchitecture.Domain.Exceptions;

namespace OnionArchitecture.Domain;
public class Auditable<TUser>:BaseEntity where TUser : User
{
    public int CreatedById { get;protected set; }
    public DateTime RecordDateTime { get;protected set; }

    public void SetAuditDetails(int createdById)
    {
        if(createdById != 0 && CreatedById != createdById)
        {
            throw new DomainException("CreatedBy already set.");
        } 
        CreatedById = createdById;
        RecordDateTime = DateTime.UtcNow.AddHours(4);
    }
}
