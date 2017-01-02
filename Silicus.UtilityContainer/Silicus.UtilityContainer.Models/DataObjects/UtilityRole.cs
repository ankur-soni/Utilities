using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class UtilityRole
    {
        [Key]
        public int ID { get; set; }
        public int UtilityID { get; set; }
        public int RoleID { get; set; }

        //[ForeignKey("UtilityId")]
        //public virtual Utility Utilities { get; set; }

        //[ForeignKey("RoleId")]
        //public virtual Role Roles { get; set; }
    }
}