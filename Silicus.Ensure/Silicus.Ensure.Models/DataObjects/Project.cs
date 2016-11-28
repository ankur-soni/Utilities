using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required]
        public string ProjectDescription { get; set; }
    }
}
