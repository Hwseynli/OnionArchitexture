﻿using OnionArchitecture.Application.Interfaces.IRepositories;
using OnionArchitecture.Domain.Entities.Documents;
using OnionArchitecture.Persistence.Context;

namespace OnionArchitecture.Persistence.Repositories;
public class DocumentRepository : Repository<Document>, IDocumentRepository
{
    public DocumentRepository(TestDbContext context) : base(context)
    {
    }
}


