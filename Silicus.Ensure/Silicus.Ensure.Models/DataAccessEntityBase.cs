using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models
{
    abstract public class DataAccessEntityBase
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}