//using HRPayroll.Domain.Interfaces;
//using HRPayroll.Application.Services;
//using Microsoft.Extensions.DependencyInjection;

//namespace HRPayroll.Application;

///// <summary>
///// Dependency injection configuration for Application layer
///// </summary>
//public static class DependencyInjection
//{
//    public static IServiceCollection AddApplication(this IServiceCollection services)
//    {
//        // Register Services
//        services.AddScoped<IEmployeeService, EmployeeService>();
//        services.AddScoped<IDepartmentService, DepartmentService>();
//        services.AddScoped<IDesignationService, DesignationService>();
//        services.AddScoped<IShiftService, ShiftService>();
//        services.AddScoped<IAttendanceService, AttendanceService>();
//        services.AddScoped<ILeaveTypeService, LeaveTypeService>();
//        services.AddScoped<ILeaveService, LeaveService>();
//        services.AddScoped<IHolidayService, HolidayService>();
//        services.AddScoped<IPayrollService, PayrollService>();
//        services.AddScoped<ITaxService, TaxService>();
//        services.AddScoped<IReportService, ReportService>();
//        services.AddScoped<IDocumentService, DocumentService>();
//        services.AddScoped<IESSService, ESSService>();

//        return services;
//    }
//}
