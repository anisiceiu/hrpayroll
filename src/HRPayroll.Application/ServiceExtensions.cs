using Microsoft.Extensions.DependencyInjection;
using HRPayroll.Domain.Interfaces;
using HRPayroll.Application.Services;

namespace HRPayroll.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDesignationService, DesignationService>();
        services.AddScoped<IShiftService, ShiftService>();
        services.AddScoped<IEmployeeShiftService, EmployeeShiftService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<ILeaveService, LeaveService>();
        services.AddScoped<ILeaveTypeService, LeaveTypeService>();
        services.AddScoped<IHolidayService, HolidayService>();
        services.AddScoped<IPayrollService, PayrollService>();
        services.AddScoped<ITaxService, TaxService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IDocumentCategoryService, DocumentCategoryService>();
        services.AddScoped<IESSService, ESSService>();
        services.AddScoped<ISalaryStructureService, SalaryStructureService>();

        // Register helper classes
        services.AddScoped<AttendanceCalculationHelper>();

        return services;
    }
}
