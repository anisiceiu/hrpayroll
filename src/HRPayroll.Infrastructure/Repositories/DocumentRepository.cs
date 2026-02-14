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
        return await _dbSet
            .Where(d => d.EmployeeId == employeeId)
            .Include(d => d.Category)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<EmployeeDocument>> GetByTypeAsync(string documentType)
    {
        return await _dbSet.Where(d => d.DocumentType == documentType).OrderByDescending(d => d.CreatedAt).ToListAsync();
    }

    /// <summary>
    /// Get documents by employee ID with category
    /// </summary>
    public async Task<IEnumerable<EmployeeDocument>> GetByEmployeeIdWithIncludesAsync(long employeeId)
    {
        return await _dbSet
            .Where(d => d.EmployeeId == employeeId)
            .Include(d => d.Category)
            .Include(d => d.Employee)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<EmployeeDocument>> GetByCategoryIdAsync(long categoryId)
    {
        return await _dbSet
            .Where(d => d.CategoryId == categoryId)
            .Include(d => d.Employee)
            .Include(d => d.Category)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<EmployeeDocument>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(d => d.Employee)
            .Include(d => d.Category)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }
}

/// <summary>
/// DocumentCategory repository implementation
/// </summary>
public class DocumentCategoryRepository : Repository<DocumentCategory>, IDocumentCategoryRepository
{
    public DocumentCategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<DocumentCategory?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(d => d.Code == code);
    }

    public async Task<IEnumerable<DocumentCategory>> GetActiveCategoriesAsync()
    {
        return await _dbSet
            .Where(d => d.IsActive)
            .OrderBy(d => d.DisplayOrder)
            .ThenBy(d => d.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Get all categories with document count
    /// </summary>
    public async Task<IEnumerable<DocumentCategory>> GetAllWithDocumentCountAsync()
    {
        return await _dbSet
            .Include(d => d.Documents)
            .OrderBy(d => d.DisplayOrder)
            .ThenBy(d => d.Name)
            .ToListAsync();
    }
}
