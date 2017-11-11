using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DocumentModel
    {
        public int DocumentID { get; set; }
        public Nullable<int> DocCatID { get; set; }
        //public Nullable<int> SubDocCatID { get; set; }
        public string Document { get; set; }
        public Nullable<bool> IsNeeded { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
