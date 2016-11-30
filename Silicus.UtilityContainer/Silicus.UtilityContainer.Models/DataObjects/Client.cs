using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Models.DataObjects
{
  public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Attention { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string AddresLine1 { get; set; }
        public string AddresLine2 { get; set; }
        public string City { get; set; }
        public string ContactNumber { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> AccountManagerID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string website { get; set; }
    }
}
