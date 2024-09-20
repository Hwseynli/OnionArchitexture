namespace OnionArchitecture.Application.Features.Queries.ViewModels.Customers;
public class CustomerDto
{
    public int CustomerId { get; set; }          // Müştərinin ID-si
    public string FirstName { get; set; }        // Müştərinin adı
    public string LastName { get; set; }         // Müştərinin soyadı
    public string Email { get; set; }            // Müştərinin e-mail ünvanı
    public List<DocumentDto> AdditionDocuments { get; set; } = new List<DocumentDto>();  // Müştərinin sənədləri
}

