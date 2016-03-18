using Silicus.ProjectTracker.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ChangeRequestDetails : BaseEntity
    {
        public ChangeRequestDetails()
        {
            IsActive = true;
        }

        [Key]
        public int ChangeRequestId { get; set; }

        public string ChangeRequestNumber { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReceivedDate { get; set; }

        public int ChangeRequestStatusId { get; set; }

        [UIHint("ComplaintStatusEditor")]
        public StatusList ChangeRequestStatusList { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [Required]
        public int WeekId { get; set; }

        public bool IsActive { get; set; }
    }
}
