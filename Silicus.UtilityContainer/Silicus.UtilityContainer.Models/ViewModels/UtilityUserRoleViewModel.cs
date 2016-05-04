using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.ViewModels
{
   public class UtilityUserRoleViewModel
    {

        public List<int> UserId { get; set; }
        public virtual List<User> User { get; set; }
      
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

      
        public int UtilityId { get; set; }
        public virtual Utility Utility { get; set; }
    }
}
