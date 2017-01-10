using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silicus.UtilityContainer.Web.Models
{
    public class RolesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool AlreadyExistsInSelectedUtility { get; set; }
    }
}