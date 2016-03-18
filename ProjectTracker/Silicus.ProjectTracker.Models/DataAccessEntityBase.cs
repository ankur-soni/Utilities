namespace Silicus.ProjectTracker.Models
{
    using System.ComponentModel.DataAnnotations;

    abstract public class DataAccessEntityBase
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string CodeSk { get; set; }

        public int Order { get; set; }
    }
}