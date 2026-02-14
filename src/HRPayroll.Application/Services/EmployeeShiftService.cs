using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Application.Services;

/// <summary>
/// EmployeeShift service implementation
/// </summary>
public class EmployeeShiftService : IEmployeeShiftService
{
    private readonly IEmployeeShiftRepository _employeeShiftRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IShiftRepository _shiftRepository;

    public EmployeeShiftService(
        IEmployeeShiftRepository employeeShiftRepository,
        IEmployeeRepository employeeRepository,
        IShiftRepository shiftRepository)
    {
        _employeeShiftRepository = employeeShiftRepository;
        _employeeRepository = employeeRepository;
        _shiftRepository = shiftRepository;
    }

    public async Task<IEnumerable<EmployeeShift>> GetAllEmployeeShiftsAsync()
    {
        return await _employeeShiftRepository.GetAllWithIncludesAsync();
    }

    public async Task<EmployeeShift?> GetEmployeeShiftByIdAsync(long id)
    {
        return await _employeeShiftRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<EmployeeShift>> GetByEmployeeIdAsync(long employeeId)
    {
        return await _employeeShiftRepository.GetByEmployeeIdWithIncludesAsync(employeeId);
    }

    public async Task<IEnumerable<EmployeeShift>> GetByShiftIdAsync(long shiftId)
    {
        return await _employeeShiftRepository.GetByShiftIdAsync(shiftId);
    }

    public async Task<EmployeeShift?> GetCurrentAssignmentAsync(long employeeId)
    {
        return await _employeeShiftRepository.GetCurrentAssignmentWithIncludesAsync(employeeId);
    }

    public async Task<EmployeeShift> AssignShiftAsync(EmployeeShift employeeShift)
    {
        // Validate employee exists
        var employee = await _employeeRepository.GetByIdAsync(employeeShift.EmployeeId);
        if (employee == null)
        {
            throw new Exception("Employee not found.");
        }

        // Validate shift exists
        var shift = await _shiftRepository.GetByIdAsync(employeeShift.ShiftId);
        if (shift == null)
        {
            throw new Exception("Shift not found.");
        }

        // Check if there's already an active assignment for this employee
        var currentAssignment = await _employeeShiftRepository.GetCurrentAssignmentAsync(employeeShift.EmployeeId);
        if (currentAssignment != null && employeeShift.EffectiveFrom <= DateTime.Today)
        {
            // Deactivate the current assignment
            currentAssignment.IsActive = false;
            currentAssignment.EffectiveTo = employeeShift.EffectiveFrom.AddDays(-1);
            await _employeeShiftRepository.UpdateAsync(currentAssignment);
        }

        // Set default values
        employeeShift.IsActive = true;
        
        return await _employeeShiftRepository.AddAsync(employeeShift);
    }

    public async Task<EmployeeShift> UpdateEmployeeShiftAsync(EmployeeShift employeeShift)
    {
        var existing = await _employeeShiftRepository.GetByIdAsync(employeeShift.Id);
        if (existing == null)
        {
            throw new Exception("Employee shift assignment not found.");
        }

        existing.ShiftId = employeeShift.ShiftId;
        existing.EffectiveFrom = employeeShift.EffectiveFrom;
        existing.EffectiveTo = employeeShift.EffectiveTo;
        existing.IsActive = employeeShift.IsActive;
        existing.Notes = employeeShift.Notes;

        return await _employeeShiftRepository.UpdateAsync(existing);
    }

    public async Task<bool> RemoveAssignmentAsync(long id)
    {
        var employeeShift = await _employeeShiftRepository.GetByIdAsync(id);
        if (employeeShift == null)
        {
            throw new Exception("Employee shift assignment not found.");
        }

        // Soft delete - just mark as inactive
        employeeShift.IsActive = false;
        await _employeeShiftRepository.UpdateAsync(employeeShift);
        return true;
    }

    public async Task<IEnumerable<EmployeeShift>> GetActiveAssignmentsAsync()
    {
        return await _employeeShiftRepository.GetActiveAssignmentsAsync();
    }
}
