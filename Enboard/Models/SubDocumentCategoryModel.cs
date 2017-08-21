using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class SubDocumentCategoryModel
    {
        public int DocCat_Id { get; set; }
        public string DocumentCategory { get; set; }
        public Nullable<int> IsNeeded { get; set; }
        public int SubDocCat_Pk { get; set; }
       
    }
}
