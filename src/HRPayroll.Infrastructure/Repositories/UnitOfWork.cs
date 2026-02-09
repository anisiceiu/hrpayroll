using HRPayroll.Domain.Interfaces;
using HRPayroll.Infrastructure.Data;

namespace HRPayroll.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    private IEmployeeRepository? _employees;
    private IDepartmentRepository? _departments;
    private IDesignationRepository? _designations;
    private IShiftRepository? _shifts;
    private IAttendanceRepository? _attendances;
    private ILeaveTypeRepository? _leaveTypes;
    private ILeaveRepository? _leaves;
    private ILeaveBalanceRepository? _leaveBalances;
    private IHolidayRepository? _holidays;
    private IDocumentRepository? _documents;
    private ISalaryStructureRepository? _salaryStructures;
    private ISalaryComponentRepository? _salaryComponents;
    private IPayrollRunRepository? _payrollRuns;
    private IPayrollDetailRepository? _payrollDetails;
    private ILoanRepository? _loans;
    private IBonusRepository? _bonuses;
    private IOvertimeRepository? _overtimes;
    private ITaxConfigRepository? _taxConfigs;
    private ITaxSlabRepository? _taxSlabs;

    public IEmployeeRepository Employees => _employees ??= new EmployeeRepository(_context);
    public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_context);
    public IDesignationRepository Designations => _designations ??= new DesignationRepository(_context);
    public IShiftRepository Shifts => _shifts ??= new ShiftRepository(_context);
    public IAttendanceRepository Attendances => _attendances ??= new AttendanceRepository(_context);
    public ILeaveTypeRepository LeaveTypes => _leaveTypes ??= new LeaveTypeRepository(_context);
    public ILeaveRepository Leaves => _leaves ??= new LeaveRepository(_context);
    public ILeaveBalanceRepository LeaveBalances => _leaveBalances ??= new LeaveBalanceRepository(_context);
    public IHolidayRepository Holidays => _holidays ??= new HolidayRepository(_context);
    public IDocumentRepository Documents => _documents ??= new DocumentRepository(_context);
    public ISalaryStructureRepository SalaryStructures => _salaryStructures ??= new SalaryStructureRepository(_context);
    public ISalaryComponentRepository SalaryComponents => _salaryComponents ??= new SalaryComponentRepository(_context);
    public IPayrollRunRepository PayrollRuns => _payrollRuns ??= new PayrollRunRepository(_context);
    public IPayrollDetailRepository PayrollDetails => _payrollDetails ??= new PayrollDetailRepository(_context);
    public ILoanRepository Loans => _loans ??= new LoanRepository(_context);
    public IBonusRepository Bonuses => _bonuses ??= new BonusRepository(_context);
    public IOvertimeRepository Overtimes => _overtimes ??= new OvertimeRepository(_context);
    public ITaxConfigRepository TaxConfigs => _taxConfigs ??= new TaxConfigRepository(_context);
    public ITaxSlabRepository TaxSlabs => _taxSlabs ??= new TaxSlabRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
