using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.Models.DataObjects
{
    using System;
    using System.Collections.Generic;

    public partial class Skill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Skill()
        {
            this.Skill1 = new HashSet<Skill>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ParentID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skill> Skill1 { get; set; }
        public virtual Skill Skill2 { get; set; }
    }
}


