# HR & Payroll Management System

A comprehensive HR & Payroll Management System built with ASP.NET Core MVC, Entity Framework Core, and SQL Server following Clean Architecture principles.

## 🏗️ Architecture

The system follows **Clean Architecture** with four main layers:

```
src/
├── HRPayroll.Domain/          # Domain Layer (Entities, Enums, Interfaces)
├── HRPayroll.Application/      # Application Layer (DTOs, Services, Mappings)
├── HRPayroll.Infrastructure/    # Infrastructure Layer (DbContext, Repositories, Identity)
└── HRPayroll.Web/             # Web Layer (Controllers, Views, wwwroot)
```

## 📦 Features

### HR Modules
- **Employee Management** - Complete CRUD operations, profile management, document handling
- **Department & Designation** - Organizational structure management
- **Attendance System** - Daily attendance, biometric import support, manual entry
- **Leave Management** - Leave types, approval workflow, balance tracking
- **Shift & Working Hours** - Flexible shift management
- **Holiday Calendar** - Configurable holiday list
- **Recruitment & Onboarding** - Job applications and employee onboarding
- **Performance Appraisal** - Employee evaluation system
- **ESS Portal** - Employee Self-Service portal

### Payroll Modules
- **Salary Structure** - Basic, allowances, deductions configuration
- **Payroll Processing** - Monthly payroll generation
- **Overtime Calculation** - Automated OT computation
- **Tax Calculation** - Bangladesh tax rules (progressive slabs)
- **Loans & Advances** - Employee loan management
- **Bonus & Incentives** - Festival bonuses, performance incentives
- **Payslip Generation** - PDF payslip support
- **Bank Transfer Export** - Export to bank formats

### Security
- **ASP.NET Identity** - User authentication
- **Role-based Authorization** - Admin, HR, Payroll, Manager, Employee roles
- **Claims-based Permissions** - Fine-grained access control

## 🛠️ Tech Stack

- **.NET 8** - Latest stable version
- **ASP.NET Core MVC** - Web framework
- **Entity Framework Core 8** - ORM with migrations
- **SQL Server** - Database
- **ASP.NET Identity** - Authentication & authorization
- **AutoMapper** - Entity-DTO mapping
- **Serilog** - Structured logging
- **AdminLTE 3** - Responsive admin dashboard
- **Bootstrap 5** - UI framework

## 📊 Database Schema

### Core Tables
- `Employees` - Employee information
- `Departments` - Organizational departments
- `Designations` - Job titles/positions
- `Shifts` - Work shifts configuration
- `Attendances` - Daily attendance records
- `LeaveTypes` - Types of leave
- `Leaves` - Leave applications
- `LeaveBalances` - Employee leave balances
- `Holidays` - Company holidays
- `SalaryStructures` - Employee salary configurations
- `PayrollRuns` - Monthly payroll processing
- `PayrollDetails` - Individual payroll entries
- `Loans` - Employee loans
- `TaxConfigs` - Tax configuration
- `TaxSlabs` - Progressive tax slabs (Bangladesh)

### Identity Tables
- `AspNetUsers` - Users
- `AspNetRoles` - Roles
- `AspNetUserRoles` - User-Role mapping

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (Express or Developer)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
```bash
git clone <repository-url>
cd hrpayroll
```

2. **Update database connection string**
Edit `src/HRPayroll.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HRPayrollDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. **Run database migrations**
```bash
dotnet ef database update --project src/HRPayroll.Infrastructure
```

4. **Run the application**
```bash
dotnet run --project src/HRPayroll.Web
```

### Default Users
After database seed, the following users are created:

| Email | Password | Role |
|-------|----------|------|
| admin@hrpayroll.com | Admin@123 | Admin |
| hr@hrpayroll.com | Hr@123 | HR |
| payroll@hrpayroll.com | Payroll@123 | Payroll |
| manager@hrpayroll.com | Manager@123 | Manager |
| employee@hrpayroll.com | Employee@123 | Employee |

## 📁 Project Structure

```
src/
├── HRPayroll.Domain/
│   ├── Common/
│   │   ├── BaseEntity.cs
│   │   └── AuditableEntity.cs
│   ├── Entities/
│   │   ├── HR/
│   │   │   ├── Employee.cs
│   │   │   ├── Department.cs
│   │   │   ├── Designation.cs
│   │   │   ├── Shift.cs
│   │   │   ├── Attendance.cs
│   │   │   ├── LeaveType.cs
│   │   │   ├── Leave.cs
│   │   │   ├── LeaveBalance.cs
│   │   │   ├── Holiday.cs
│   │   │   ├── EmployeeDocument.cs
│   │   │   └── ...
│   │   └── Payroll/
│   │       ├── SalaryStructure.cs
│   │       ├── PayrollRun.cs
│   │       ├── PayrollDetail.cs
│   │       ├── Loan.cs
│   │       ├── TaxConfig.cs
│   │       └── ...
│   ├── Enums/
│   │   ├── EmployeeEnums.cs
│   │   ├── AttendanceEnums.cs
│   │   ├── LeaveEnums.cs
│   │   ├── PayrollEnums.cs
│   │   └── HolidayEnums.cs
│   └── Interfaces/
│       ├── IRepositories/
│       └── IServices/
│
├── HRPayroll.Application/
│   ├── DTOs/
│   │   ├── EmployeeDTOs.cs
│   │   ├── AttendanceDTOs.cs
│   │   ├── LeaveDTOs.cs
│   │   └── ...
│   ├── Profiles/
│   │   └── MappingProfile.cs
│   ├── Services/
│   │   ├── EmployeeService.cs
│   │   ├── DepartmentService.cs
│   │   ├── AttendanceService.cs
│   │   ├── LeaveService.cs
│   │   ├── PayrollService.cs
│   │   ├── TaxService.cs
│   │   └── ...
│   └── ServiceExtensions.cs
│
├── HRPayroll.Infrastructure/
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   └── DatabaseInitializer.cs
│   ├── Identity/
│   │   ├── ApplicationUser.cs
│   │   └── ApplicationRole.cs
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   └── UnitOfWork.cs
│   └── InfrastructureExtensions.cs
│
└── HRPayroll.Web/
    ├── Controllers/
    │   ├── HomeController.cs
    │   ├── AccountController.cs
    │   ├── EmployeeController.cs
    │   ├── DepartmentController.cs
    │   ├── AttendanceController.cs
    │   ├── LeaveController.cs
    │   ├── PayrollController.cs
    │   ├── ReportController.cs
    │   ├── ESSController.cs
    │   └── ...
    ├── Views/
    │   ├── Shared/
    │   │   └── _Layout.cshtml
    │   ├── Home/
    │   ├── Employee/
    │   ├── Attendance/
    │   ├── Payroll/
    │   └── ...
    ├── wwwroot/
    │   ├── css/
    │   ├── js/
    │   ├── plugins/
    │   └── dist/
    ├── appsettings.json
    └── Program.cs
```

## 🏦 Bangladesh Tax Rules Configuration

The system includes built-in support for Bangladesh tax rules:

```csharp
// Tax Slabs (Financial Year 2024)
- 0 - 350,000: 0%
- 350,001 - 450,000: 5%
- 450,001 - 650,000: 10%
- 650,001 - 900,000: 15%
- 900,001 - 1,150,000: 20%
- 1,150,001+: 25%

// Standard Deduction: 450,000 BDT
// Max Investment Rebate: 600,000 BDT (15% of taxable income)
```

## 📝 License

This project is licensed under the MIT License.

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request
