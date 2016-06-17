using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public class ResourceSkillLevel
    {
        public int ID { get; set; }
        [ForeignKey("Resource")]
        public int ResourceID { get; set; }
        public virtual Resource Resource { get; set; }

        [ForeignKey("Skill")]
        public int SkillID { get; set; }
        public virtual Skill Skill { get; set; }
        //public int SkillLevel { get; set; }

    }
}
