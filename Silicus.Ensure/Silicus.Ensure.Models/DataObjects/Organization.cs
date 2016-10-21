using System.ComponentModel.DataAnnotations;

namespace Silicus.Ensure.Models.DataObjects
{
    public class Organization
    {    

        /// <summary>
        /// Identifier.
        /// </summary>
        public int OrganizationId { get; set; }

        [Required]
        public string Name { get; set; }
               
        public string Description { get; set; }
               
    }
}