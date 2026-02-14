using HRPayroll.Application.Services;
using HRPayroll.Domain.Entities.HR;

namespace HRPayroll.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalDesignations { get; set; }
        public int TotalShifts { get; set; }
        public List<Attendance> Attendances { get; set; }=new List<Attendance>();
        public int TodayPresent { get; set; }
        public int TodayAbsent { get; set; }
        public int TodayLate { get; set; }
        public int TodayHalfDay { get; set; }
        public int TodayLeave { get; set; }
    }
}
