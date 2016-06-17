using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class ResourceType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsBillable { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsTrackMissingTime { get; set; }
        public bool IsSendEmailsForMissingTime { get; set; }
        public bool IsApproveOwnTime { get; set; }
        public string PayCategory { get; set; }
        public bool IsAllTimeOffReasons { get; set; }
        public bool IsPersonalTime { get; set; }
        public bool IsIllness { get; set; }
        public bool IsVacation { get; set; }
        public bool IsOptionalOrFloatingHoliday { get; set; }
        public bool IsJuryDuty { get; set; }
        public bool IsActive { get; set; }
        public bool IsRDCBasedOnFixedSalary { get; set; }
    }
}
