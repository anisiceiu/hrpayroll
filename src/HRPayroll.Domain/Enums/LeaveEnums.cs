namespace HRPayroll.Domain.Enums;

/// <summary>
/// Leave request status
/// </summary>
public enum LeaveStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4,
    Withdrawn = 5
}

/// <summary>
/// Types of leave
/// </summary>
public enum LeaveType
{
    Annual = 1,
    Sick = 2,
    Casual = 3,
    Maternity = 4,
    Paternity = 5,
    Marriage = 6,
    Bereavement = 7,
    Compensatory = 8,
    Unpaid = 9,
    Study = 10,
    Medical = 11,
    Festival = 12,
    CarryForward = 13,
    WorkFromHome = 14
}

/// <summary>
/// Half day portion selection
/// </summary>
public enum HalfDayPortion
{
    Morning = 1,
    Afternoon = 2
}
