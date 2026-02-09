using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRPayroll.Infrastructure.Repositories;

/// <summary>
/// Document repository implementation
/// </summary>
public class DocumentRepository : Repository<EmployeeDocument>, IDocumentRepository
{
    public DocumentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<EmployeeDocument>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.Where(d => d.EmployeeId == employeeId).OrderByDescending(d => d.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<EmployeeDocument>> GetByTypeAsync(string documentType)
    {
        return await _dbSet.Where(d => d.DocumentType == documentType).OrderByDescending(d => d.CreatedAt).ToListAsync();
    }
}
