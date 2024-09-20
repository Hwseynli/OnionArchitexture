using OnionArchitecture.Domain.Entities.Documents;

namespace OnionArchitecture.Application.Interfaces.IRepositories;
public interface IAdditionDocumentRepository : IRepository<AdditionDocument>
{
    Task<List<AdditionDocument>> GetDocumentsByCustomerIdAsync(int customerId);
}
