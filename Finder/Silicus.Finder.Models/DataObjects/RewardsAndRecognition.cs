using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Models.DataObjects
{
    public class RewardsAndRecognition
    {
        [Key]
        public int RewardsAndRecognitionId { get; set; }

        [Display(Name = "Reward / Recognition ")]
        public string RewardsAndRecognitionName { get; set; }

        public virtual ICollection<EmployeeRewards> EmployeeRewards { get; set; }
    }
}
