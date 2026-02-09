namespace HRPayroll.Domain.Enums;

/// <summary>
/// Holiday types
/// </summary>
public enum HolidayType
{
    National = 1,
    Festival = 2,
    Religious = 3,
    Optional = 4,
    Weekly = 5,
    Special = 6
}

/// <summary>
/// Recruitment status
/// </summary>
public enum RecruitmentStatus
{
    Open = 1,
    Closed = 2,
    OnHold = 3,
    Draft = 4
}

/// <summary>
/// Application status
/// </summary>
public enum ApplicationStatus
{
    Applied = 1,
    Screening = 2,
    Interview = 3,
    Selected = 4,
    Rejected = 5,
    Withdrawn = 6,
    OnHold = 7
}

/// <summary>
/// Onboarding status
/// </summary>
public enum OnboardingStatus
{
    NotStarted = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}

/// <summary>
/// Performance appraisal status
/// </summary>
public enum AppraisalStatus
{
    NotStarted = 1,
    SelfReview = 2,
    ManagerReview = 3,
    HRReview = 4,
    Completed = 5,
    Cancelled = 6
}
