using HRPayroll.Domain.Entities.Payroll;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRPayroll.Infrastructure.Repositories;

/// <summary>
/// SalaryStructure repository implementation
/// </summary>
public class SalaryStructureRepository : Repository<SalaryStructure>, ISalaryStructureRepository
{
    public SalaryStructureRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get all salary structures with navigation properties (Employee and Components)
    /// </summary>
    public async Task<IEnumerable<SalaryStructure>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(s => s.Employee)
                .ThenInclude(e => e!.Department)
            .Include(s => s.Components)
            .ToListAsync();
    }

    /// <summary>
    /// Get salary structure by ID with navigation properties
    /// </summary>
    public async Task<SalaryStructure?> GetByIdWithIncludesAsync(long id)
    {
        return await _dbSet
            .Include(s => s.Employee)
                .ThenInclude(e => e!.Department)
            .Include(s => s.Components)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    /// <summary>
    /// Get salary structure by employee ID with navigation properties
    /// </summary>
    public async Task<SalaryStructure?> GetByEmployeeIdWithIncludesAsync(long employeeId)
    {
        return await _dbSet
            .Include(s => s.Employee)
                .ThenInclude(e => e!.Department)
            .Include(s => s.Components)
            .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);
    }

    public async Task<SalaryStructure?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.Name == code);
    }

    public async Task<SalaryStructure?> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.EmployeeId == employeeId);
    }

    public async Task<IEnumerable<SalaryStructure>> GetActiveStructuresAsync()
    {
        return await _dbSet.Where(s => s.IsActive).ToListAsync();
    }

    /// <summary>
    /// Get active salary structures with navigation properties
    /// </summary>
    public async Task<IEnumerable<SalaryStructure>> GetActiveStructuresWithIncludesAsync()
    {
        return await _dbSet
            .Where(s => s.IsActive)
            .Include(s => s.Employee)
                .ThenInclude(e => e!.Department)
            .Include(s => s.Components)
            .ToListAsync();
    }
}

/// <summary>
/// SalaryComponent repository implementation
/// </summary>
public class SalaryComponentRepository : Repository<SalaryComponent>, ISalaryComponentRepository
{
    public SalaryComponentRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get all salary components with navigation properties
    /// </summary>
    public async Task<IEnumerable<SalaryComponent>> GetAllWithIncludesAsync()
    {
        return await _dbSet
            .Include(sc => sc.SalaryStructure)
                .ThenInclude(ss => ss!.Employee)
            .ToListAsync();
    }

    public async Task<IEnumerable<SalaryComponent>> GetByStructureIdAsync(long structureId)
    {
        return await _dbSet.Where(sc => sc.SalaryStructureId == structureId).ToListAsync();
    }

    /// <summary>
    /// Get salary components by structure ID with navigation properties
    /// </summary>
    public async Task<IEnumerable<SalaryComponent>> GetByStructureIdWithIncludesAsync(long structureId)
    {
        return await _dbSet
            .Where(sc => sc.SalaryStructureId == structureId)
            .Include(sc => sc.SalaryStructure)
                .ThenInclude(ss => ss!.Employee)
            .ToListAsync();
    }
}

/// <summary>
/// PayrollRun repository implementation
/// </summary>
public class PayrollRunRepository : Repository<PayrollRun>, IPayrollRunRepository
{
    public PayrollRunRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PayrollRun?> GetByRunCodeAsync(string runCode)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Month.ToString() == runCode);
    }

    public async Task<PayrollRun?> GetByMonthAndYearAsync(int month, int year)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Month == month && p.Year == year);
    }

    public async Task<IEnumerable<PayrollRun>> GetByYearAsync(int year)
    {
        return await _dbSet.Where(p => p.Year == year).OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<PayrollRun>> GetPendingApprovalAsync()
    {
        return await _dbSet.Where(p => p.Status == PayrollRunStatus.PendingApproval).ToListAsync();
    }
}

/// <summary>
/// PayrollDetail repository implementation
/// </summary>
public class PayrollDetailRepository : Repository<PayrollDetail>, IPayrollDetailRepository
{
    public PayrollDetailRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PayrollDetail>> GetByPayrollRunIdAsync(long payrollRunId)
    {
        return await _dbSet.Where(p => p.PayrollRunId == payrollRunId).ToListAsync();
    }

    public async Task<IEnumerable<PayrollDetail>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.Where(p => p.EmployeeId == employeeId).OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<PayrollDetail?> GetByEmployeeAndRunIdAsync(long employeeId, long payrollRunId)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.PayrollRunId == payrollRunId);
    }
}

/// <summary>
/// Loan repository implementation
/// </summary>
public class LoanRepository : Repository<Loan>, ILoanRepository
{
    public LoanRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Loan>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.Where(l => l.EmployeeId == employeeId).OrderByDescending(l => l.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetActiveLoansByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.Where(l => l.EmployeeId == employeeId && l.Status == LoanStatus.Active).ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status)
    {
        return await _dbSet.Where(l => l.Status == status).ToListAsync();
    }
}

/// <summary>
/// Bonus repository implementation
/// </summary>
public class BonusRepository : Repository<Bonus>, IBonusRepository
{
    public BonusRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Bonus>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.Where(b => b.EmployeeId == employeeId).OrderByDescending(b => b.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Bonus>> GetByPayrollRunIdAsync(long payrollRunId)
    {
        return await _dbSet.Where(b => b.Id == payrollRunId).ToListAsync();
    }

    public async Task<IEnumerable<Bonus>> GetByBonusTypeAsync(BonusType bonusType)
    {
        return await _dbSet.Where(b => b.Type == bonusType).OrderByDescending(b => b.CreatedAt).ToListAsync();
    }
}

/// <summary>
/// Overtime repository implementation
/// </summary>
public class OvertimeRepository : Repository<Overtime>, IOvertimeRepository
{
    public OvertimeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Overtime>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _dbSet.Where(o => o.EmployeeId == employeeId).OrderByDescending(o => o.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Overtime>> GetByDateRangeAsync(long employeeId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet.Where(o => o.EmployeeId == employeeId && o.OvertimeDate >= startDate && o.OvertimeDate <= endDate)
            .OrderByDescending(o => o.OvertimeDate).ToListAsync();
    }

    public async Task<IEnumerable<Overtime>> GetPendingApprovalAsync(long approverId)
    {
        return await _dbSet.Where(o => o.Status == OvertimeStatus.Pending).ToListAsync();
    }

    public async Task<IEnumerable<Overtime>> GetApprovedNotPaidAsync()
    {
        return await _dbSet.Where(o => o.Status == OvertimeStatus.Approved).ToListAsync();
    }
}

/// <summary>
/// TaxConfig repository implementation
/// </summary>
public class TaxConfigRepository : Repository<TaxConfig>, ITaxConfigRepository
{
    public TaxConfigRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<TaxConfig?> GetByTaxYearAsync(int taxYear)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.TaxYear == taxYear);
    }

    public async Task<TaxConfig?> GetCurrentConfigAsync()
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.IsActive);
    }
}

/// <summary>
/// TaxSlab repository implementation
/// </summary>
public class TaxSlabRepository : Repository<TaxSlab>, ITaxSlabRepository
{
    public TaxSlabRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TaxSlab>> GetByConfigIdAsync(long configId)
    {
        return await _dbSet.Where(ts => ts.TaxConfigId == configId).OrderBy(ts => ts.Id).ToListAsync();
    }

    public async Task<IEnumerable<TaxSlab>> GetCurrentTaxSlabsAsync()
    {
        var currentConfig = await _dbSet.FirstOrDefaultAsync(t => t.IsActive);
        if (currentConfig == null)
            return new List<TaxSlab>();

        return await _dbSet.Where(ts => ts.TaxConfigId == currentConfig.Id).OrderBy(ts => ts.Id).ToListAsync();
    }
}
