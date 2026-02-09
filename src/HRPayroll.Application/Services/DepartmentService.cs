using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Application.Services;

/// <summary>
/// Department service implementation
/// </summary>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        return await _departmentRepository.GetAllAsync();
    }

    public async Task<Department?> GetDepartmentByIdAsync(long id)
    {
        return await _departmentRepository.GetByIdAsync(id);
    }

    public async Task<Department> CreateDepartmentAsync(Department department)
    {
        // Check for duplicate code
        var existing = await _departmentRepository.GetByCodeAsync(department.Code);
        if (existing != null)
        {
            throw new Exception("A department with this code already exists.");
        }

        department.IsActive = true;
        return await _departmentRepository.AddAsync(department);
    }

    public async Task<Department> UpdateDepartmentAsync(Department department)
    {
        var existing = await _departmentRepository.GetByIdAsync(department.Id);
        if (existing == null)
        {
            throw new Exception("Department not found.");
        }

        existing.Name = department.Name;
        existing.NameBN = department.NameBN;
        existing.Description = department.Description;
        existing.ParentDepartmentId = department.ParentDepartmentId;
        existing.HeadOfDepartmentId = department.HeadOfDepartmentId;
        existing.IsActive = department.IsActive;

        return await _departmentRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteDepartmentAsync(long id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new Exception("Department not found.");
        }

        department.IsActive = false;
        await _departmentRepository.UpdateAsync(department);
        return true;
    }

    public async Task<IEnumerable<Department>> GetActiveDepartmentsAsync()
    {
        return await _departmentRepository.GetActiveDepartmentsAsync();
    }
}

/// <summary>
/// Designation service implementation
/// </summary>
public class DesignationService : IDesignationService
{
    private readonly IDesignationRepository _designationRepository;

    public DesignationService(IDesignationRepository designationRepository)
    {
        _designationRepository = designationRepository;
    }

    public async Task<IEnumerable<Designation>> GetAllDesignationsAsync()
    {
        return await _designationRepository.GetAllAsync();
    }

    public async Task<Designation?> GetDesignationByIdAsync(long id)
    {
        return await _designationRepository.GetByIdAsync(id);
    }

    public async Task<Designation> CreateDesignationAsync(Designation designation)
    {
        var existing = await _designationRepository.GetByCodeAsync(designation.Code);
        if (existing != null)
        {
            throw new Exception("A designation with this code already exists.");
        }

        designation.IsActive = true;
        return await _designationRepository.AddAsync(designation);
    }

    public async Task<Designation> UpdateDesignationAsync(Designation designation)
    {
        var existing = await _designationRepository.GetByIdAsync(designation.Id);
        if (existing == null)
        {
            throw new Exception("Designation not found.");
        }

        existing.Name = designation.Name;
        existing.NameBN = designation.NameBN;
        existing.Description = designation.Description;
        existing.DepartmentId = designation.DepartmentId;
        existing.Grade = designation.Grade;
        existing.Level = designation.Level;
        existing.BaseSalary = designation.BaseSalary;
        existing.IsActive = designation.IsActive;

        return await _designationRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteDesignationAsync(long id)
    {
        var designation = await _designationRepository.GetByIdAsync(id);
        if (designation == null)
        {
            throw new Exception("Designation not found.");
        }

        designation.IsActive = false;
        await _designationRepository.UpdateAsync(designation);
        return true;
    }

    public async Task<IEnumerable<Designation>> GetByDepartmentIdAsync(long departmentId)
    {
        return await _designationRepository.GetByDepartmentIdAsync(departmentId);
    }

    public async Task<IEnumerable<Designation>> GetActiveDesignationsAsync()
    {
        return await _designationRepository.GetActiveDesignationsAsync();
    }
}

/// <summary>
/// Shift service implementation
/// </summary>
public class ShiftService : IShiftService
{
    private readonly IShiftRepository _shiftRepository;

    public ShiftService(IShiftRepository shiftRepository)
    {
        _shiftRepository = shiftRepository;
    }

    public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
    {
        return await _shiftRepository.GetAllAsync();
    }

    public async Task<Shift?> GetShiftByIdAsync(long id)
    {
        return await _shiftRepository.GetByIdAsync(id);
    }

    public async Task<Shift> CreateShiftAsync(Shift shift)
    {
        var existing = await _shiftRepository.GetByCodeAsync(shift.Code);
        if (existing != null)
        {
            throw new Exception("A shift with this code already exists.");
        }

        // Calculate working hours
        var breakTimeHours = shift.BreakTimeMinutes / 60.0m;
        var totalHours = (shift.EndTime - shift.StartTime).TotalHours;
        if (totalHours < 0) totalHours += 24; // Handle night shift
        shift.WorkingHours = Math.Round((decimal)totalHours - breakTimeHours, 2);

        shift.IsActive = true;
        return await _shiftRepository.AddAsync(shift);
    }

    public async Task<Shift> UpdateShiftAsync(Shift shift)
    {
        var existing = await _shiftRepository.GetByIdAsync(shift.Id);
        if (existing == null)
        {
            throw new Exception("Shift not found.");
        }

        existing.Name = shift.Name;
        existing.Code = shift.Code;
        existing.StartTime = shift.StartTime;
        existing.EndTime = shift.EndTime;
        existing.BreakTimeMinutes = shift.BreakTimeMinutes;
        existing.GraceTimeMinutes = shift.GraceTimeMinutes;
        existing.IsNightShift = shift.IsNightShift;
        existing.IsActive = shift.IsActive;

        // Recalculate working hours
        var breakTimeHours = existing.BreakTimeMinutes / 60.0m;
        var totalHours = (existing.EndTime - existing.StartTime).TotalHours;
        if (totalHours < 0) totalHours += 24;
        existing.WorkingHours = Math.Round((decimal)totalHours - breakTimeHours, 2);

        return await _shiftRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteShiftAsync(long id)
    {
        var shift = await _shiftRepository.GetByIdAsync(id);
        if (shift == null)
        {
            throw new Exception("Shift not found.");
        }

        shift.IsActive = false;
        await _shiftRepository.UpdateAsync(shift);
        return true;
    }

    public async Task<IEnumerable<Shift>> GetActiveShiftsAsync()
    {
        return await _shiftRepository.GetActiveShiftsAsync();
    }
}
