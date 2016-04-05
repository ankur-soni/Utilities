
using System.ComponentModel.DataAnnotations;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class Utility
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public byte[] UtilityIcon { get; set; }
    }
}
