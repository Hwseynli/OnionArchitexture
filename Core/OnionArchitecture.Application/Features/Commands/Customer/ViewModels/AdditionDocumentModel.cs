using Microsoft.AspNetCore.Http;
namespace OnionArchitecture.Application.Features.Commands.Customer.ViewModels
{
    public class AdditionDocumentModel
    {
        public int DocumentTypeId { get; set; }
        public string Other { get; set; }
        public List<IFormFile> Documents { get; set; }
    }
}

