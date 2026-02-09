namespace HRPayroll.Domain.Enums;

/// <summary>
/// Salary component types
/// </summary>
public enum ComponentType
{
    Basic = 1,
    Allowance = 2,
    Deduction = 3,
    Tax = 4,
    Contribution = 5,
    Bonus = 6,
    Overtime = 7,
    Arrears = 8,
    Reimbursement = 9
}

/// <summary>
/// Salary component calculation types
/// </summary>
public enum CalculationType
{
    Fixed = 1,
    Percentage = 2,
    Formula = 3
}

/// <summary>
/// Payroll run status
/// </summary>
public enum PayrollRunStatus
{
    Draft = 1,
    PendingApproval = 2,
    Approved = 3,
    Processed = 4,
    Paid = 5,
    Cancelled = 6
}

/// <summary>
/// Payment status
/// </summary>
public enum PaymentStatus
{
    Pending = 1,
    Paid = 2,
    Failed = 3,
    OnHold = 4,
    Refunded = 5
}

/// <summary>
/// Loan types
/// </summary>
public enum LoanType
{
    PFLoan = 1,
    FestivalAdvance = 2,
    PersonalLoan = 3,
    CarLoan = 4,
    HouseBuildingLoan = 5,
    MotorcycleLoan = 6,
    EmergencyLoan = 7
}

/// <summary>
/// Loan status
/// </summary>
public enum LoanStatus
{
    Pending = 1,
    Active = 2,
    Completed = 3,
    Defaulted = 4,
    Cancelled = 5,
    OnHold = 6,
    Rejected = 7
}

/// <summary>
/// Bonus types
/// </summary>
public enum BonusType
{
    FestivalBonus = 1,
    PerformanceBonus = 2,
    YearEndBonus = 3,
    ProductionBonus = 4,
    ProjectBonus = 5,
    ReferralBonus = 6,
    SpotBonus = 7
}

/// <summary>
/// Overtime status
/// </summary>
public enum OvertimeStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}
