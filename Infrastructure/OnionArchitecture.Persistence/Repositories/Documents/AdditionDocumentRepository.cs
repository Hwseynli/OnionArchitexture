using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities.Documents;
using OnionArchitecture.Persistence.Context;

namespace OnionArchitecture.Persistence.Repositories;
public class AdditionDocumentRepository : Repository<AdditionDocument>, IAdditionDocumentRepository
{
    private readonly TestDbContext _context;
    public AdditionDocumentRepository(TestDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<AdditionDocument>> GetDocumentsByCustomerIdAsync(int customerId)
    {
        // Müştərinin sənədlərini əldə etmək üçün müvafiq sorğu yazırıq
        return await _context.AdditionDocuments
                             .Where(doc => doc.CustomerId == customerId)
                             .ToListAsync();
    }
}

