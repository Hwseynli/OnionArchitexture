using OnionArchitecture.Domain.Common;

namespace OnionArchitecture.Domain.Entities.Roles;
public class Role:IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void SetDetails(string name)
    {
        Name = name;
    }
}