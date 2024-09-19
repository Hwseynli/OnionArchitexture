using OnionArchitecture.Domain.Common;

namespace OnionArchitecture.Domain.Entities.Documents;
public class Document : Editable<User>, IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; private set; }
    public string Path { get; private set; }
    public AdditionDocument AdditionDocument { get; private set; }
    public int AdditionDocumentId { get; private set; }

    public void SetDetails(string name, string url)
    {
        Name = name;
        Path = url;
    }
}

