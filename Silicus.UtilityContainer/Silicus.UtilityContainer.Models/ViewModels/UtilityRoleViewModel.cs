using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.UtilityContainer.Models.ViewModels
{
    public class UtilityRoleViewModel
    {
        public int UtilityId { get; set; }
        public List<int> RoleIds { get; set; }
    }
}