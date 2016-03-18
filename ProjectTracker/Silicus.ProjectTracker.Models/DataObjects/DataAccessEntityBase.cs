
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.ProjectTracker.Models.DataObjects
{
    abstract public class DataAccessEntityBase
    {
        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
