using System.ComponentModel.DataAnnotations;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
    }
}
