namespace HRPayroll.Domain.Enums;

/// <summary>
/// Employee employment status
/// </summary>
public enum EmployeeStatus
{
    Active = 1,
    Inactive = 2,
    OnLeave = 3,
    Terminated = 4,
    Resigned = 5,
    Retired = 6,
    Suspended = 7
}

/// <summary>
/// Employee employment type
/// </summary>
public enum EmploymentType
{
    Permanent = 1,
    Contractual = 2,
    Intern = 3,
    Probation = 4,
    Temporary = 5,
    PartTime = 6
}

/// <summary>
/// Employee employment category
/// </summary>
public enum EmploymentCategory
{
    Staff = 1,
    Officer = 2,
    Executive = 3,
    SeniorExecutive = 4,
    Management = 5
}

/// <summary>
/// Work location types
/// </summary>
public enum WorkLocation
{
    HeadOffice = 1,
    BranchOffice = 2,
    Remote = 3,
    Field = 4,
    Factory = 5
}

/// <summary>
/// Gender types
/// </summary>
public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3,
    PreferNotToSay = 4
}

/// <summary>
/// Marital status
/// </summary>
public enum MaritalStatus
{
    Single = 1,
    Married = 2,
    Divorced = 3,
    Widowed = 4,
    Separated = 5
}

/// <summary>
/// User roles for authorization
/// </summary>
public enum UserRole
{
    SystemAdmin = 1,
    HRManager = 2,
    HRExecutive = 3,
    PayrollManager = 4,
    PayrollExecutive = 5,
    DepartmentManager = 6,
    FinanceManager = 7,
    Employee = 8,
    Interviewer = 9
}
