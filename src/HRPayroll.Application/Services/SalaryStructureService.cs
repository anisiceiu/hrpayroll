using HRPayroll.Domain.Entities.Payroll;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Application.Services;

/// <summary>
/// SalaryStructure service implementation
/// </summary>
public class SalaryStructureService : ISalaryStructureService
{
    private readonly ISalaryStructureRepository _salaryStructureRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public SalaryStructureService(
        ISalaryStructureRepository salaryStructureRepository,
        IEmployeeRepository employeeRepository)
    {
        _salaryStructureRepository = salaryStructureRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<SalaryStructure>> GetAllSalaryStructuresAsync()
    {
        return await _salaryStructureRepository.GetAllAsync();
    }

    public async Task<SalaryStructure?> GetSalaryStructureByIdAsync(long id)
    {
        return await _salaryStructureRepository.GetByIdAsync(id);
    }

    public async Task<SalaryStructure?> GetByEmployeeIdAsync(long employeeId)
    {
        return await _salaryStructureRepository.GetByEmployeeIdAsync(employeeId);
    }

    public async Task<SalaryStructure> CreateSalaryStructureAsync(SalaryStructure salaryStructure)
    {
        // Calculate GrossSalary
        salaryStructure.GrossSalary = salaryStructure.BasicSalary +
                                       salaryStructure.HouseRentAllowance +
                                       salaryStructure.TransportAllowance +
                                       salaryStructure.MedicalAllowance +
                                       salaryStructure.ConveyanceAllowance +
                                       salaryStructure.OtherAllowances;

        // Calculate TotalDeductions
        var pfAmount = (salaryStructure.BasicSalary * salaryStructure.ProvidentFundPercentage) / 100;
        salaryStructure.TotalDeductions = pfAmount +
                                         salaryStructure.TaxDeduction +
                                         salaryStructure.OtherDeductions;

        // Calculate NetSalary
        salaryStructure.NetSalary = salaryStructure.GrossSalary - salaryStructure.TotalDeductions;

        salaryStructure.IsActive = true;
        return await _salaryStructureRepository.AddAsync(salaryStructure);
    }

    public async Task<SalaryStructure> UpdateSalaryStructureAsync(SalaryStructure salaryStructure)
    {
        var existing = await _salaryStructureRepository.GetByIdAsync(salaryStructure.Id);
        if (existing == null)
        {
            throw new Exception("Salary structure not found.");
        }

        // Update fields
        existing.EmployeeId = salaryStructure.EmployeeId;
        existing.Name = salaryStructure.Name;
        existing.BasicSalary = salaryStructure.BasicSalary;
        existing.HouseRentAllowance = salaryStructure.HouseRentAllowance;
        existing.TransportAllowance = salaryStructure.TransportAllowance;
        existing.MedicalAllowance = salaryStructure.MedicalAllowance;
        existing.ConveyanceAllowance = salaryStructure.ConveyanceAllowance;
        existing.OtherAllowances = salaryStructure.OtherAllowances;
        existing.ProvidentFundPercentage = salaryStructure.ProvidentFundPercentage;
        existing.TaxDeduction = salaryStructure.TaxDeduction;
        existing.OtherDeductions = salaryStructure.OtherDeductions;
        existing.EffectiveFrom = salaryStructure.EffectiveFrom;
        existing.EffectiveTo = salaryStructure.EffectiveTo;
        existing.Notes = salaryStructure.Notes;

        // Recalculate GrossSalary
        existing.GrossSalary = existing.BasicSalary +
                               existing.HouseRentAllowance +
                               existing.TransportAllowance +
                               existing.MedicalAllowance +
                               existing.ConveyanceAllowance +
                               existing.OtherAllowances;

        // Recalculate TotalDeductions
        var pfAmount = (existing.BasicSalary * existing.ProvidentFundPercentage) / 100;
        existing.TotalDeductions = pfAmount +
                                   existing.TaxDeduction +
                                   existing.OtherDeductions;

        // Recalculate NetSalary
        existing.NetSalary = existing.GrossSalary - existing.TotalDeductions;

        return await _salaryStructureRepository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteSalaryStructureAsync(long id)
    {
        var salaryStructure = await _salaryStructureRepository.GetByIdAsync(id);
        if (salaryStructure == null)
        {
            throw new Exception("Salary structure not found.");
        }

        await _salaryStructureRepository.DeleteAsync(salaryStructure);
        return true;
    }

    public async Task<IEnumerable<SalaryStructure>> GetActiveStructuresAsync()
    {
        return await _salaryStructureRepository.GetActiveStructuresAsync();
    }
}
