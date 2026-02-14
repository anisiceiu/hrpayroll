using HRPayroll.Infrastructure.Data;
using HRPayroll.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Configuration
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Register repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDesignationRepository, DesignationRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<IEmployeeShiftRepository, EmployeeShiftRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<ILeaveRepository, LeaveRepository>();
        services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
        services.AddScoped<IHolidayRepository, HolidayRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ISalaryStructureRepository, SalaryStructureRepository>();
        services.AddScoped<ISalaryComponentRepository, SalaryComponentRepository>();
        services.AddScoped<IPayrollRunRepository, PayrollRunRepository>();
        services.AddScoped<IPayrollDetailRepository, PayrollDetailRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IBonusRepository, BonusRepository>();
        services.AddScoped<IOvertimeRepository, OvertimeRepository>();
        services.AddScoped<ITaxConfigRepository, TaxConfigRepository>();
        services.AddScoped<ITaxSlabRepository, TaxSlabRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Generic Repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
