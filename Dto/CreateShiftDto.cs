using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Dto
{
    public class CreateShiftDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [Required]
        public List<Guid> EmployeeId { get; set; }
        [Required]
        public DateTime ShiftDate { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}