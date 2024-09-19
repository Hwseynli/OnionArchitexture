using OnionArchitecture.Domain.Common;

namespace OnionArchitecture.Domain.Entities.Documents;

public class AdditionDocument : Editable<User>, IBaseEntity
{
    public int Id { get; set; }
    public string? Other { get; private set; }
    public List<Document> Documents { get; private set; }
    public DocumentType DocumentType { get; private set; }
    public int CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public AdditionDocument()
    {
        Documents = new List<Document>();
    }
    public void SetDetails(DocumentType documentType, string? other)
    {
        DocumentType = documentType;
        Other = other;
    }
}

