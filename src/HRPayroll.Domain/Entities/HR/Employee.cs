using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRPayroll.Domain.Common;
using HRPayroll.Domain.Enums;
using HRPayroll.Domain.Entities.Payroll;

namespace HRPayroll.Domain.Entities.HR;

/// <summary>
/// Employee entity representing an employee in the organization
/// </summary>
public class Employee : AuditableEntity
{
    //[Required]
    [MaxLength(20)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    public string? PhoneNumber => Phone;

    public DateTime? DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public MaritalStatus? MaritalStatus { get; set; }

    [MaxLength(5)]
    public string? BloodGroup { get; set; }

    [MaxLength(500)]
    public string? NationalId { get; set; }

    [MaxLength(500)]
    public string? PassportNo { get; set; }

    [MaxLength(500)]
    public string? TinNo { get; set; }

    public bool IsTaxExempted { get; set; }

    public DateTime? DateOfJoining { get; set; }

    public DateTime? JoiningDate => DateOfJoining;

    public DateTime? DateOfConfirmation { get; set; }

    public DateTime? ConfirmationDate => DateOfConfirmation;

    public DateTime? TerminationDate { get; set; }

    [MaxLength(500)]
    public string? PresentAddress { get; set; }

    [MaxLength(500)]
    public string? PermanentAddress { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(50)]
    public string? Country { get; set; } = "Bangladesh";

    [MaxLength(20)]
    public string? PostalCode { get; set; }

    [MaxLength(100)]
    public string? BankName { get; set; }

    [MaxLength(50)]
    public string? BankAccountNo { get; set; }

    [MaxLength(50)]
    public string? BranchName { get; set; }

    [MaxLength(100)]
    public string? RoutingNo { get; set; }

    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

    public EmploymentType EmploymentType { get; set; }

    public EmploymentCategory EmploymentCategory { get; set; }

    public WorkLocation WorkLocation { get; set; }

    [MaxLength(500)]
    public string? ProfilePhotoPath { get; set; }

    public string? ProfilePhoto { get; set; }

    // Foreign Keys
    public long? DepartmentId { get; set; }
    public long? DesignationId { get; set; }
    public long? ShiftId { get; set; }
    public long? ManagerId { get; set; }
    public long? SupervisorId { get; set; }

    // Navigation Properties
    [ForeignKey(nameof(DepartmentId))]
    public virtual Department? Department { get; set; }

    [ForeignKey(nameof(DesignationId))]
    public virtual Designation? Designation { get; set; }

    [ForeignKey(nameof(ShiftId))]
    public virtual Shift? Shift { get; set; }

    [ForeignKey(nameof(ManagerId))]
    public virtual Employee? Manager { get; set; }

    [ForeignKey(nameof(SupervisorId))]
    public virtual Employee? Supervisor { get; set; }

    public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
    public virtual ICollection<PayrollDetail> PayrollDetails { get; set; } = new List<PayrollDetail>();
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public virtual ICollection<Bonus> Bonuses { get; set; } = new List<Bonus>();
    public virtual ICollection<Overtime> Overtimes { get; set; } = new List<Overtime>();
    public virtual ICollection<PerformanceAppraisal> PerformanceAppraisals { get; set; } = new List<PerformanceAppraisal>();
    public virtual ICollection<EmployeeDocument> Documents { get; set; } = new List<EmployeeDocument>();
    public virtual ICollection<Recruitment> Recruitments { get; set; } = new List<Recruitment>();
    public virtual SalaryStructure? SalaryStructure { get; set; }
}
