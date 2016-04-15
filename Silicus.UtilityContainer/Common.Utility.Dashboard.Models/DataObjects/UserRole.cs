using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int UtilityId { get; set; }

       // [ForeignKey("UserId")]
       // public virtual User Users { get; set; }

        [ForeignKey("UtilityId")]
        public virtual Utility Utilities { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Roles { get; set; }
    }
}
