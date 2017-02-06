using Silicus.Ensure.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Models.DataObjects
{
    public class UserApplicationDetails
    {
        public int UserApplicationDetailsId { get; set; }

        public int? PanelMemberId { get; set; }

        public int PositionId { get; set; }

        public int UserId { get; set; }

        public string RequisitionId { get; set; }

        public string ClientName { get; set; }

        public string Technology { get; set; }

        public int TotalExperienceInYear { get; set; }

        public int TotalExperienceInMonth { get; set; }

        public int RelevantExperienceInYear { get; set; }

        public int RelevantExperienceInMonth { get; set; }

        public string CurrentCompany { get; set; }

        public string CurrentTitle { get; set; }

        public CandidateStatus CandidateStatus { get; set; }

        public string ResumePath { get; set; }

        public string ResumeName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public virtual Position Position { get; set; }

        public virtual User User { get; set; }
        
    }
}
