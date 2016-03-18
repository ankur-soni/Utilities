using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Silicus.Finder.Models.DataObjects
{
    public class EmployeeRewards
    {
        [Key]
        public int EmployeeRewardId { get; set; }
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        [Display(Name = "Reward / Recognition")]
        public int RewardsAndRecognitionId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Remote("ValidateDateEqualOrLess", "Employee" ,HttpMethod = "Post",
        ErrorMessage = "Date isn't equal or less than current date.")]
        [Display(Name = "Given On")]
        [DataType(DataType.Date)]
     
        public DateTime? GivenOn { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employees { get; set; }

        [ForeignKey("RewardsAndRecognitionId")]
        public virtual RewardsAndRecognition RewardsAndRecognitions { get; set; }
    }
}