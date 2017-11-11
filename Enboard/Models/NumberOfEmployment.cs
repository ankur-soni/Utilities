using System.ComponentModel.DataAnnotations;
namespace Models
{
    public class NumberOfEmployment
    {
        [Display(Name = "Number Of Employments")]
        [Range(0, 20, ErrorMessage = "Please enter total number of Employments including current.(In case of fresher put 0)")]
        public int? NumberOfEmployments { get; set; }
    }
}