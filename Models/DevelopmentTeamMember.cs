using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class DevelopmentTeamMember
    {
        public Guid Id { get; set; }

        [Required]
        public Guid DevelopmentTeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Department { get; set; }  // Bộ phận
        public string Position { get; set; }    // Chức vụ
        public string Role { get; set; }        // Vai trò trong phần mềm

        [Required]
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }


        // Quan hệ 1-n: mỗi member thuộc 1 team
        public DevelopmentTeam DevelopmentTeam { get; set; }

            // 👇 Thêm hai dòng sau:
        public string Email { get; set; }
        public string Password { get; set; }
    }
}