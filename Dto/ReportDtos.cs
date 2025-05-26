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

    public class SoftwareReportDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
    public class SoftwareByEmployeeReportDto
    {
        public Guid SoftwareId { get; set; }
        public string SoftwareName { get; set; }
        public DateTime StartDate { get; set; }
        public string Role { get; set; }
        public string Position { get; set; }
    }
}