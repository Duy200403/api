using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Dto
{
    public class DevelopmentTeamDto
    {
        public Guid Id { get; set; }
        [Required]
        public Guid SoftwareId { get; set; }
        [Required]
        public string MemberName { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set;}

        // Optional: Thông tin phần mềm liên quan
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SoftwareDto Software { get; set; }
    }
}