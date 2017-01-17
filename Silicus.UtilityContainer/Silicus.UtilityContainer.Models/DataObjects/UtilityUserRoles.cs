using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class UtilityUserRoles
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int UtilityId { get; set; }
        public virtual Utility Utility { get; set; }
        public bool IsActive { get; set; }
        //    public virtual Utility Utilities { get; set; }
        //    public virtual Role Roles { get; set; }
    }
}
