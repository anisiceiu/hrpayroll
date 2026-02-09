namespace HRPayroll.Domain.Enums;

/// <summary>
/// Attendance status types
/// </summary>
public enum AttendanceStatus
{
    Present = 1,
    Absent = 2,
    Late = 3,
    HalfDay = 4,
    Leave = 5,
    Holiday = 6,
    Weekend = 7,
    OnDuty = 8,
    Compensatory = 9
}

/// <summary>
/// Attendance entry method
/// </summary>
public enum EntryType
{
    Manual = 1,
    Biometric = 2,
    Auto = 3,
    Mobile = 4,
    Web = 5
}
