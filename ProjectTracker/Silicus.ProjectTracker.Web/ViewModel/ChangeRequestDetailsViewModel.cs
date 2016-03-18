using System;
using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Core;
using System.ComponentModel.DataAnnotations;
using Silicus.ProjectTracker.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.ProjectTracker.Web.ViewModel
{
    public class ChangeRequestDetailsViewModel : BaseEntity
    {
        public int ChangeRequestId { get; set; }

        public string ChangeRequestNumber { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReceivedDate { get; set; }

        public int ChangeRequestStatusId { get; set; }

        [UIHint("ComplaintStatusEditor")]
        public StatusList ChangeRequestStatusList { get; set; }

        public string ChangeRequestName { get; set; }

        public int ProjectId { get; set; }
        
        public int WeekId { get; set; } 
    }
}