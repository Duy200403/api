using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto
{
    public class UpdateDevelopmentTeamDto
    {
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
    }
}