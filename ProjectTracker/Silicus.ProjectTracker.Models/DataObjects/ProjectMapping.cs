using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Silicus.ProjectTracker.Models.DataObjects
{
    public class ProjectMapping : BaseEntity
    {
       [ScaffoldColumn(false)]
       public int MappingId { get; set; }    

       public string UserName { get; set; }

       [ForeignKey("Project")]
       public int ProjectId { get; set; }

       public bool IsDeleted { get; set; }

       public virtual Project Project { get; set; }               
    }
}
