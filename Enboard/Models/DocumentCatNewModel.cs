using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class DocumentCatNewModel
    {
        public int DocCat_Pk { get; set; }
        public int DocCat_Id { get; set; }
        public string DocumentName { get; set; }
        public Nullable<int> IsNeeded { get; set; }
    }
}
