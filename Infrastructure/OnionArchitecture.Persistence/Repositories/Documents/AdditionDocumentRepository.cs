using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities.Documents;
using OnionArchitecture.Persistence.Context;

namespace OnionArchitecture.Persistence.Repositories;
public class AdditionDocumentRepository : Repository<AdditionDocument>, IAdditionDocumentRepository
{
    public AdditionDocumentRepository(TestDbContext context) : base(context)
    {
    }
}

