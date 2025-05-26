using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class DevelopmentTeam
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
        public string UpdatedBy { get; set; }

        // Navigation property: Một DevelopmentTeam thuộc về một Software
        public virtual Software Software { get; set; }
        // Quan hệ 1-n với thành viên
        public ICollection<DevelopmentTeamMember> Members { get; set; } = new List<DevelopmentTeamMember>();

    }
}