using System.ComponentModel.DataAnnotations;

namespace Silicus.Finder.Models
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