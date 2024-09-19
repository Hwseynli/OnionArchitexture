
using OnionArchitecture.Domain.Common;
using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Domain.Entities;
public class Customer:Editable<User>,IBaseEntity
{
    public int Id { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public List<AdditionDocument> AdditionDocuments { get; private set; }
    public Customer()
    {
        AdditionDocuments = new List<AdditionDocument>();
    }

    public void SetDetails(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}

