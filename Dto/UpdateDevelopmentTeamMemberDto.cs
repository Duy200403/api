using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto
{
    public class UpdateDevelopmentTeamMemberDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string UpdatedBy { get; set; }
    }
}