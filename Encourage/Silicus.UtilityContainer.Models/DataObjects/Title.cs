using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
    public partial class Title
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public bool IsActive { get; set; }
        public Nullable<decimal> MinBillable { get; set; }
        public Nullable<decimal> MinChargeble { get; set; }
        public Nullable<decimal> MinProductive { get; set; }
        public Nullable<decimal> MaxBillable { get; set; }
        public Nullable<decimal> MaxChargeble { get; set; }
        public Nullable<decimal> MaxProductive { get; set; }
        public int LocationID { get; set; }
        public Nullable<int> ResourceTypeID { get; set; }
    }
}
