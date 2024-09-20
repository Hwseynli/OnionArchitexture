namespace OnionArchitecture.Application.Features.Queries.ViewModels.Customers;
public class DocumentDto
{
    public int DocumentId { get; set; }         // Sənədin ID-si
    public string DocumentName { get; set; }    // Sənədin adı
    public string DocumentType { get; set; }     // Sənədin tipi (məsələn, "Id", "Other")
}

