using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto
{
    public class ShiftCountReportDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class EmployeeShiftsReportDto
    {
        public Guid Id { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class EmployeeShiftTotalReportDto
    {
        public Guid EmployeeId { get; set; }
        public int TotalShifts { get; set; }
    }

    public class WorkEfficiencyReportDto
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int TotalShifts { get; set; }
        public double TotalHours { get; set; }
    }
}